using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    public static MoneySystem Instance;

    public TextMeshProUGUI moneyText;
    private int money = 50000;

    private int prixDeVenteProduit;

    public Dictionary<string, ProduitData> dicoProduit = new Dictionary<string, ProduitData>();

    private void Start()
    {
        dicoProduit.Add("Carotte", new ProduitData(500, 750, 1));
        dicoProduit.Add("Salade", new ProduitData(3000, 4500, 1));
        dicoProduit.Add("Tomate", new ProduitData(8500, 12000, 1));
        dicoProduit.Add("Steack", new ProduitData(15000, 22000, 1));
        dicoProduit.Add("Framboise", new ProduitData(50000, 75000, 1));
        dicoProduit.Add("Pain", new ProduitData(85000, 125000, 1));
        dicoProduit.Add("Pomme", new ProduitData(150000, 200000, 1));
        dicoProduit.Add("Champi", new ProduitData(350000, 475000, 1));
        dicoProduit.Add("Pasteque", new ProduitData(1000000, 10000000, 1));

        moneyText.text = money.ToString();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool Acheter(string produit)
    {
        int prixProduit = dicoProduit[produit].PrixAchat;

        if (money >= prixProduit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ArgentEnlever(string produit)
    {
        int prixProduit = dicoProduit[produit].PrixAchat;

        money -= prixProduit;

        moneyText.text = money.ToString();
    }

    public void Vendre(string produit)
    {
        prixDeVenteProduit = dicoProduit[produit].PrixVente;

        money += prixDeVenteProduit;

        moneyText.text = money.ToString();
    }

    public void RendArgent(string produit)
    {
        int rendArgent = dicoProduit[produit].PrixAchat;

        money += rendArgent;

        moneyText.text = money.ToString();
    }

    public void MultiplicateurVente()
    {
        money += prixDeVenteProduit * 2;

        moneyText.text = money.ToString();
    }

    public void VenteCharged(int chargeAmount)
    {
        money += prixDeVenteProduit * chargeAmount;

        moneyText.text = money.ToString();
    }
}
