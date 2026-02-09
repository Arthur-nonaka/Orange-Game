using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public GameObject prefab;
    public float price;
    public float priceMultiplier = 1f;
}
