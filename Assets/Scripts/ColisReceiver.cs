using NootColis.Logic;
using TMPro;
using UnityEngine;

public class ColisReceiver : MonoBehaviour
{
    public Transform content;
    public GameObject prefabRequest;

    public RequestReader transcripter;

    private int index = 0;

    private void Start()
    {
        NootColisAPI.GetStreamOfColis("Louka"); // recevoir colis
    }

    private void Update()
    {
        if (NootColisAPI.GetInboxCount("Louka") > 0)
        {
            Colis colis = NootColisAPI.PopColis("Louka");

            GameObject colisInstancier = Instantiate(prefabRequest, content);

            ColisData data = colisInstancier.GetComponent<ColisData>();
            data.contenu = colis.contenu;
            data.Init(transcripter);

            TextMeshProUGUI text = colisInstancier.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            text.text = index.ToString();

            index++;

            Debug.Log("J'ai reçu ! : " + colis.contenu);
        }
    }
}
