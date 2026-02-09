using UnityEngine;
using UnityEngine.EventSystems;

public class CropsDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject cropPrefabs;
    public CropsData cropsData;

    private GameObject objet;

    private bool isDragging = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);
        RaycastHit hit;

        Debug.Log("est ce que je peut acheté " + gameObject.tag);

        bool peutAcheter = MoneySystem.Instance.Acheter(gameObject.tag);

        if (peutAcheter)
        {
            MoneySystem.Instance.ArgentEnlever(gameObject.tag);

            isDragging = true;

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 worldPos = ray.GetPoint(distance);
                objet = Instantiate(cropPrefabs, worldPos, Quaternion.identity);
                objet.transform.Translate(new Vector3(0, 0, -1));
            }
        }
        else
        {
            return;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);

        bool peutAcheter = MoneySystem.Instance.Acheter(gameObject.tag);

        if (plane.Raycast(ray, out float distance) && isDragging)
        {
            objet.transform.position = ray.GetPoint(distance);
            objet.transform.Translate(new Vector3(0, 0, -1));
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;

        bool peutAcheter = MoneySystem.Instance.Acheter(gameObject.tag);

        if (Physics.Raycast(ray, out hit) && isDragging)
        {
            if (hit.collider.CompareTag("Plot"))
            {
                Transform centerPoint = hit.collider.transform.Find("PointCentral");

                if (centerPoint != null && hit.collider.GetComponent<PlotState>().containCrops == false)
                {
                    GameObject crop = Instantiate(cropPrefabs, centerPoint.position, Quaternion.identity);
                    crop.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    crop.transform.parent = hit.collider.transform.Find("PointCentral");
                    hit.collider.GetComponent<PlotState>().containCrops = true;
                }
                else
                {
                    MoneySystem.Instance.RendArgent(gameObject.tag);
                    Debug.Log("a était destroy");
                    Destroy(objet);
                    return;
                }

                isDragging = false;
            }
        }
        Destroy(objet);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MiniTooltipUI.Instance != null && cropsData != null)
            MiniTooltipUI.Instance.Show(cropsData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (MiniTooltipUI.Instance != null)
            MiniTooltipUI.Instance.HideInstant();
    }
}
