using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class ItemStock
{
    public string itemName;
    public TextMeshProUGUI stockText;
}

public class RequestReader : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI mainText;
    public List<ItemStock> itemStocks;

    // <-- stock réel
    public Dictionary<string, int> stockQuantities;
    public Dictionary<string, ItemStock> stockDictionary;

    private GameObject source;
    private ColisData currentSelected;

    private void Awake()
    {
        stockDictionary = new Dictionary<string, ItemStock>();
        stockQuantities = new Dictionary<string, int>();

        foreach (var item in itemStocks)
        {
            stockDictionary.Add(item.itemName, item);
            // Parse le texte initial pour le stock réel
            string[] split = item.stockText.text.Split(':');
            int qty = 0;
            if (split.Length == 2)
                int.TryParse(split[1].Trim(), out qty);

            stockQuantities.Add(item.itemName, qty);
            UpdateStockUI(item.itemName);
        }
    }

    public void ShowColis(ColisData data, GameObject source)
    {
        if (currentSelected == data)
        {
            mainText.text = "";
            currentSelected = null;
            return;
        }

        currentSelected = data;
        mainText.text = data.contenu;
        this.source = source;
    }

    public void SendRequest()
    {
        if (string.IsNullOrEmpty(mainText.text)) return;

        string[] split = mainText.text.Split('_');
        if (split.Length != 2) return;

        string itemName = split[0];
        if (!int.TryParse(split[1], out int requestedAmount)) return;

        if (!stockQuantities.ContainsKey(itemName)) return;

        if (stockQuantities[itemName] >= requestedAmount)
        {
            stockQuantities[itemName] -= requestedAmount;
            UpdateStockUI(itemName);

            Debug.Log($"Demande envoyée : {itemName} - {requestedAmount}");

            GameObject.Find("SendItemToRequest").GetComponent<SendItemToRequest>().Send(source);

            if (source != null) Destroy(source);
            source = null;
            mainText.text = "";
            currentSelected = null;

        }
        else
        {
            Debug.Log($"Pas assez de {itemName} en stock !");
        }
    }

    // Mise à jour simple du texte UI
    public void UpdateStockUI(string itemName)
    {
        if (stockDictionary.ContainsKey(itemName))
        {
            stockDictionary[itemName].stockText.text = $"{itemName} : {stockQuantities[itemName]}";
        }
    }

    // Fonction pour ajouter un item depuis OutilsDeStockage
    public void AddItem(string itemName, int amount = 1)
    {
        if (!stockQuantities.ContainsKey(itemName))
        {
            stockQuantities[itemName] = amount;
            Debug.LogWarning($"Nouvel item ajouté : {itemName} ({amount})");
        }
        else
        {
            stockQuantities[itemName] += amount;
        }

        UpdateStockUI(itemName);
    }
}