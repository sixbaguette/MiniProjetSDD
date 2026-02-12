using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColisTranscripter : MonoBehaviour
{
    public TextMeshProUGUI prefabShow;

    private Transform selectZone;

    private GameObject colis;

    private void Start()
    {
        selectZone = GameObject.Find("CommandSelected").transform;
    }

    public void GetColisReceiver()
    {
        colis = GameObject.Find("ColisReceiver").GetComponent<ColisReceiver>().colisInstancier;
    }

    public void RequestToggle()
    {
        if (GameObject.Find("ColisReceiver").GetComponent<ColisReceiver>().colisInstancier.GetComponent<Toggle>().isOn)
        {
            TextMeshProUGUI text = Instantiate(prefabShow, selectZone);

            text.text = colis.GetComponent<ColisData>().contenu;
        }
    }
}
