using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public Inventory inventory;

    public void UpgradeCapacity(int amount)
    {
        inventory.maxSize = amount;
    }
}
