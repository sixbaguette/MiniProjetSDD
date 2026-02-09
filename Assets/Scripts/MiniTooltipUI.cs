using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniTooltipUI : MonoBehaviour
{
    public static MiniTooltipUI Instance;

    [Header("Refs")]
    public CanvasGroup canvasGroup;
    public RectTransform background;     // <- drag "Background"
    public RectTransform content;        // <- drag "Content"
    public TextMeshProUGUI titleText;    // <- drag
    public TextMeshProUGUI descriptionText; // <- drag

    [Header("Style")]
    public Vector2 padding = new Vector2(12f, 10f); // x=left/right, y=top/bottom
    public float lineSpacing = 6f;                  // espace entre titre et description
    public float maxWidth = 280f;                   // largeur max du tooltip
    public Vector2 offset = new Vector2(18f, -18f);
    public float appearDelay = 0.35f;
    public Sprite backgroundSprite;                 // optionnel (9-slice)

    RectTransform self;
    bool visible;

    void Awake()
    {
        Instance = this;
        self = GetComponent<RectTransform>();
        if (!canvasGroup) canvasGroup = GetComponent<CanvasGroup>();
        if (!canvasGroup) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // Auto-find si oublié
        if (!background) background = transform.Find("Background") as RectTransform;
        if (!content && background) content = background.Find("Content") as RectTransform;

        // Fix anchors/pivots
        SetupRT(self);
        SetupRT(background);
        SetupRT(content);
        SetupRT(titleText ? (RectTransform)titleText.transform : null);
        SetupRT(descriptionText ? (RectTransform)descriptionText.transform : null);

        // Texte: wrap ON, auto-size OFF, rich text OK
        if (titleText)
        {
            titleText.textWrappingMode = TextWrappingModes.Normal;
            titleText.richText = true;
            titleText.enableAutoSizing = false;
        }
        if (descriptionText)
        {
            descriptionText.textWrappingMode = TextWrappingModes.Normal;
            descriptionText.richText = true;
            descriptionText.enableAutoSizing = false;
        }

        // Met le Canvas tout devant
        var cv = GetComponentInParent<Canvas>();
        if (cv)
        {
            cv.renderMode = RenderMode.ScreenSpaceOverlay;
            cv.sortingOrder = 9999;
        }

        // Background visuel propre
        var img = background ? background.GetComponent<Image>() : null;
        if (img)
        {
            img.raycastTarget = false;
            if (backgroundSprite) img.sprite = backgroundSprite;
            img.type = Image.Type.Sliced; // si 9-slice
        }

        HideInstant();
    }

    void SetupRT(RectTransform rt)
    {
        if (!rt) return;
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
    }

    void Update()
    {
        if (visible)
        {
            self.position = Input.mousePosition + (Vector3)offset;
        }
    }

    public void Show(CropsData crop)
    {
        if (!crop) return;

        if (titleText)
            titleText.text = $"<b>{crop.cropName}</b>";

        if (descriptionText)
            descriptionText.text = BuildDesc(crop);

        ResizeToFit();

        gameObject.SetActive(true);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        canvasGroup.alpha = 1f;

        visible = true;
    }

    public void Hide()
    {

        // sécurité : ne pas lancer de coroutine si déjà inactif
        if (!gameObject.activeInHierarchy)
            return;

        visible = false;
    }

    public void HideInstant()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        gameObject.SetActive(false);
        visible = false;
    }

    // Dimensionnement manuel
    void ResizeToFit()
    {
        if (!background || !content) return;

        // Largeur cible
        float maxTextWidth = Mathf.Max(50f, maxWidth - padding.x * 2f);

        // Preferred size des textes (contraints par largeur)
        Vector2 tSize = Vector2.zero, dSize = Vector2.zero;

        if (titleText)
        {
            // largeur imposée → hauteur auto
            tSize = titleText.GetPreferredValues(titleText.text, maxTextWidth, Mathf.Infinity);
            tSize.x = Mathf.Min(tSize.x, maxTextWidth);
        }

        if (descriptionText)
        {
            dSize = descriptionText.GetPreferredValues(descriptionText.text, maxTextWidth, Mathf.Infinity);
            dSize.x = Mathf.Min(dSize.x, maxTextWidth);
        }

        float contentW = Mathf.Clamp(Mathf.Max(tSize.x, dSize.x), 50f, maxWidth - padding.x * 2f);
        float contentH = 0f;

        // Positionner les blocs dans Content
        if (titleText)
        {
            var rt = (RectTransform)titleText.transform;
            rt.anchoredPosition = Vector2.zero; // top-left du Content
            rt.sizeDelta = new Vector2(contentW, tSize.y);
            contentH += tSize.y;
        }

        if (descriptionText)
        {
            var rt = (RectTransform)descriptionText.transform;
            float y = contentH > 0f ? contentH + lineSpacing : 0f;
            rt.anchoredPosition = new Vector2(0f, -y);
            rt.sizeDelta = new Vector2(contentW, dSize.y);
            contentH = y + dSize.y;
        }

        // Taille du Content
        content.sizeDelta = new Vector2(contentW, contentH);

        // Taille du Background (padding)
        float bgW = contentW + padding.x * 2f;
        float bgH = contentH + padding.y * 2f;
        background.sizeDelta = new Vector2(bgW, bgH);

        // Décaler Content à l’intérieur du Background (padding)
        content.anchoredPosition = new Vector2(padding.x, -padding.y);
    }

    string BuildDesc(CropsData crop)
    {
        return
            $"<color=#cccccc>Prix achat :</color> {crop.buyPrice}\n" +
            $"<color=#cccccc>Prix vente :</color> {crop.sellPrice}\n" +
            $"<color=#cccccc>Temps de pousse :</color> {crop.growTime} s\n" +
            $"<color=#99ff99>Chance mutation :</color> {(crop.mutationChance * 100f):F1}%\n\n" +
            $"{crop.description}";
    }
}
