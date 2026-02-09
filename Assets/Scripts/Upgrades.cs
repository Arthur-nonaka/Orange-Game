using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public Inventory inventory;

    public OrangeSpawner orangeSpawner;

    public GameObject orangePrefab;
    public Transform treeSpawnPoint;
    public Item orangeItem;

    public void UpgradeCapacity(int amount)
    {
        inventory.maxSize = amount;
    }

    public void UpgradeOrangeMax(int amount)
    {
        orangeSpawner.maxItems = amount;
    }

    public void UpgradeOrangeRate(float amount)
    {
        orangeSpawner.spawnInterval = amount;
    }

    public void UpgradeOrangeValue(float amount)
    {
        orangeItem.priceMultiplier = amount;
    }

    public void CreateNewTree()
    {
        if (orangePrefab != null && treeSpawnPoint != null)
        {
            Vector3 randomPosition =
                treeSpawnPoint.position
                + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            Instantiate(orangePrefab, randomPosition, Quaternion.identity);
        }
    }
}
