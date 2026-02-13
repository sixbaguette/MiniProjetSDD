using UnityEngine;
using UnityEngine.UI;

public class ColisData : MonoBehaviour
{
    public string contenu;

    private Button button;
    private RequestReader transcripter;

    public void Init(RequestReader t)
    {
        transcripter = t;
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        transcripter.ShowColis(this, gameObject);
    }
}