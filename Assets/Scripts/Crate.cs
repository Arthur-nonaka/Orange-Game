using UnityEngine;

public class Crate : MonoBehaviour
{
    public Grabbable item;
    public Grabbable itemPrefab;
    public float quantity;
    public float maxQuantity;
    public TMPro.TextMeshProUGUI quantityText;

    public Grabbable GetItem()
    {
        if (quantity > 0)
        {
            quantity--;
            Grabbable item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            UpdateUI();
            return item;
        }
        else
        {
            return null;
        }
    }

    public bool CanInsertItem(float newQuantity)
    {
        return quantity + newQuantity <= maxQuantity;
    }

    public bool IsMax()
    {
        return quantity >= maxQuantity;
    }

    public void InsertItem(Grabbable item, float quantity)
    {
        if (!this.item)
        {
            this.quantity += quantity;
            if (this.quantity > maxQuantity)
            {
                this.quantity = maxQuantity;
            }
        }
        else
        {
            this.item = item;
            this.quantity = quantity;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (quantityText != null)
        {
            quantityText.text = quantity.ToString("0") + "/" + maxQuantity.ToString("0");
        }
    }
}
