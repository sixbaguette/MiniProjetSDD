using NootColis.Logic;
using UnityEngine;

public class SendItemToRequest : MonoBehaviour
{
    public void Send(GameObject source)
    {
        NootColisAPI.SendColis("Louka", "Samuel", source.GetComponent<ColisData>().contenu);
    }
}
