using UnityEngine;

public class ItemShopSlot : MonoBehaviour
{    
    public int ItemID;
    public int Price;
    public int Quantity;

    [Header("UI Elements")]
    public TMPro.TextMeshProUGUI PriceText;
    public TMPro.TextMeshProUGUI QuantityText;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        PriceText.text = "Price: $" + Price.ToString();
        QuantityText.text = "Quantity: " + Quantity.ToString();
    }

    public void BuyItem()
    {
        if (Quantity > 0)
        {
            Quantity--;
            UpdateUI();
            // Add logic to deduct coins from player
        }
    }
}
