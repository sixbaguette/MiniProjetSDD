using UnityEngine;
using UnityEngine.EventSystems;

public class OutilsDeVente : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject objet;

    public GameObject outils;

    private bool outilsVente = false;

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

                    if (!hit.collider.transform.GetChild(1).GetChild(0).GetComponent<CropsGrowSystem>().isGrowing)
                    {
                        MoneySystem.Instance.Vendre(hit.collider.transform.GetChild(1).GetChild(0).tag);

                        if (hit.collider.transform.GetChild(1).GetChild(0).GetComponent<MutationSystem>().isMutated)
                        {
                            MoneySystem.Instance.Vendre(hit.collider.transform.GetChild(1).GetChild(0).tag);
                        }

                        Destroy(parent.GetChild(1).GetChild(0).gameObject);
                        hit.collider.GetComponent<PlotState>().containCrops = false;
                    }
                    else
                    {
                        Debug.Log("pas encore prêt pour être vendu");
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
