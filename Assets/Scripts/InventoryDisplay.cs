using TMPro;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    public TextMeshProUGUI inventoryText;
    public Inventory playerInventory;

    void Start()
    {
        playerInventory.OnInventoryChanged += UpdateInventoryDisplay;
        UpdateInventoryDisplay(playerInventory.GetTotalItemCount());
    }

    void UpdateInventoryDisplay(float newAmount)
    {
        inventoryText.text = $"{newAmount}";
    }

    void OnDestroy()
    {
        playerInventory.OnInventoryChanged -= UpdateInventoryDisplay;
    }
}
