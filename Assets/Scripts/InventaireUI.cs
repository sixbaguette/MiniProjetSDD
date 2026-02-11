using UnityEngine;

public class InventaireUI : MonoBehaviour
{
    private bool isActive = false;

    public GameObject InvPanel;

    private void Start()
    {
        InvPanel.SetActive(false);
    }

    public void InventorySwitch()
    {
        isActive = !isActive;

        InvPanel.SetActive(isActive);
    }
}
