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

    private Dictionary<string, ItemStock> stockDictionary;

    private GameObject source;

    private ColisData currentSelected;

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

    private void Awake()
    {
        stockDictionary = new Dictionary<string, ItemStock>();
        foreach (var item in itemStocks)
        {
            stockDictionary.Add(item.itemName, item);
        }
    }

    public void SendRequest()
    {
        if (string.IsNullOrEmpty(mainText.text))
            return;

        string[] split = mainText.text.Split('_');
        if (split.Length != 2)
            return;

        string itemName = split[0];
        int requestedAmount;
        if (!int.TryParse(split[1], out requestedAmount))
            return;

        if (!stockDictionary.ContainsKey(itemName))
            return;

        ItemStock stockItem = stockDictionary[itemName];

        string[] stockSplit = stockItem.stockText.text.Split(':');
        if (stockSplit.Length != 2)
            return;

        int currentStock;
        if (!int.TryParse(stockSplit[1].Trim(), out currentStock))
            return;

        if (currentStock >= requestedAmount)
        {
            currentStock -= requestedAmount;
            stockItem.stockText.text = itemName + " : " + currentStock;
            Debug.Log($"Demande envoyée : {itemName} - {requestedAmount}");

            Destroy(source);
            this.source = null;
            mainText.text = "";
        }
        else
        {
            Debug.Log($"Pas assez de {itemName} en stock !");
        }
    }
}