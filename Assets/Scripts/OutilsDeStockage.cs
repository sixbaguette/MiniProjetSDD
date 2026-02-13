using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutilsDeStockage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject objet;

    public GameObject outils;

    public TextMeshProUGUI invText;

    [SerializeField]
    private StringToTMPDictionary dicoTag;

    private Dictionary<TextMeshProUGUI, int> dicoNombre;

    private void Start()
    {
        dicoNombre = new Dictionary<TextMeshProUGUI, int>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 worldPos = ray.GetPoint(distance);
            objet = Instantiate(outils, worldPos, Quaternion.identity);
            objet.transform.Translate(new Vector3(0, 0, -1));
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
        {
            objet.transform.position = ray.GetPoint(distance);
            objet.transform.Translate(new Vector3(0, 0, -1));
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Plot"))
            {
                Transform centerPoint = hit.collider.transform.Find("PointCentral");

                if (centerPoint != null && hit.collider.GetComponent<PlotState>().containCrops == true)
                {
                    Transform parent = hit.collider.transform;

                    GameObject plante = parent.GetChild(1).GetChild(0).gameObject;

                    invText = dicoTag.Map[plante.tag];

                    if (dicoNombre.ContainsKey(invText) && parent.GetChild(1).GetChild(0).GetComponent<CropsGrowSystem>().isGrowing == false)
                    {
                        dicoNombre[invText] += 1;
                        invText.text = $"{plante.tag} : " + (dicoNombre[invText]);
                        Destroy(plante);
                        hit.collider.GetComponent<PlotState>().containCrops = false;
                    }
                    else if (parent.GetChild(1).GetChild(0).GetComponent<CropsGrowSystem>().isGrowing == false)
                    {
                        dicoNombre.Add(invText, 1);
                        invText.text = $"{plante.tag} : " + (dicoNombre[invText]);
                        Destroy(plante);
                        hit.collider.GetComponent<PlotState>().containCrops = false;
                    }

                    Destroy(objet);
                }
                else
                {
                    Debug.Log("a était destroy");
                    Destroy(objet);
                    return;
                }
            }
        }
        Destroy(objet);
    }
}

[Serializable]
public struct DictionaryEntry
{
    public string Key;
    public TextMeshProUGUI Value;
}


// 2. Le dictionnaire s�rialisable
[Serializable]
public class StringToTMPDictionary : ISerializationCallbackReceiver
{
    // C'est cette liste que Unity affichera proprement dans l'Inspecteur
    [SerializeField]
    private List<DictionaryEntry> list = new List<DictionaryEntry>();


    // Le vrai dictionnaire utilis� en code (non s�rialis� par Unity)
    private Dictionary<string, TextMeshProUGUI> dictionary = new Dictionary<string, TextMeshProUGUI>();


    // Accesseur pour manipuler le dictionnaire comme un dictionnaire standard
    public Dictionary<string, TextMeshProUGUI> Map => dictionary;


    // Avant la sauvegarde : Unity lit le dictionnaire pour remplir la liste
    public void OnBeforeSerialize()
    {
        // Optionnel : Synchroniser uniquement si le dictionnaire a �t� modifi� en code
        // list.Clear();
        // foreach (var pair in dictionary)
        // {
        //     list.Add(new DictionaryEntry { Key = pair.Key, Value = pair.Value });
        // }
    }


    // Apr�s le chargement (ou modification dans l'Inspecteur)
    public void OnAfterDeserialize()
    {
        dictionary.Clear();
        foreach (var entry in list)
        {
            if (entry.Key != null && !dictionary.ContainsKey(entry.Key))
            {
                dictionary.Add(entry.Key, entry.Value);
            }
        }
    }
}
