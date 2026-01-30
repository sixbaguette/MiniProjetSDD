using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonBouton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject cropPrefabs;

    private GameObject objet;

    private Dictionary<string, int> dicoGrille = new Dictionary<string, int>();

    public void OnBeginDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 worldPos = ray.GetPoint(distance);
            objet = Instantiate(cropPrefabs, worldPos, Quaternion.identity);
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

                if (centerPoint != null && hit.collider.GetComponent<PlotState>().containCrops == false)
                {
                    GameObject crop = Instantiate(cropPrefabs, centerPoint.position, Quaternion.identity);
                    crop.transform.parent = hit.collider.transform.Find("PointCentral");
                    hit.collider.GetComponent<PlotState>().containCrops = true;

                    if (dicoGrille.ContainsKey(crop.name))
                    {
                        dicoGrille[crop.name] = dicoGrille[crop.name] + 1;
                        Debug.Log("dico a mis a jour");
                    }
                    else
                    {
                        dicoGrille.Add(crop.name, 1);
                        Debug.Log("dico a créé");
                    }
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Plot"))
                {
                    Transform centerPoint = hit.collider.transform.Find("PointCentral");

                    if (centerPoint != null && hit.collider.GetComponent<PlotState>().containCrops == true)
                    {
                        Transform parent = hit.collider.transform;
                        Destroy(parent.GetChild(0).gameObject);
                    }
                }
            }
        }
    }
}
