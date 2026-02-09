// Spawner.cs
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    public Item itemToSpawn;
    public float spawnInterval = 5f;
    public int maxItems;
    public Transform spawnPoint;
    public float raycastDistance = 10f;

    [Header("Spawns")]
    public GameObject CatSpawn;
    public GameObject CrateSpawn;

    public List<GameObject> spawnedItems = new List<GameObject>();
    protected float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && spawnedItems.Count < maxItems)
        {
            SpawnItem();
            timer = 0f;
        }
    }

    protected virtual void SpawnItem()
    {
        Vector3 randomDirection = Random.onUnitSphere;
        Vector3 rayOrigin = spawnPoint.position + randomDirection * raycastDistance;

        if (Physics.Raycast(rayOrigin, -randomDirection, out RaycastHit hit, raycastDistance * 2))
        {
            GameObject itemObject = Instantiate(itemToSpawn.prefab, hit.point, Quaternion.identity);

            Grabbable grabbable = itemObject.GetComponent<Grabbable>();
            if (grabbable != null)
            {
                grabbable.spawner = this;
            }

            spawnedItems.Add(itemObject);
            OnItemSpawned(itemObject);
        }
    }

    protected virtual void OnItemSpawned(GameObject item) { }

    public virtual void ItemCollected(GameObject grabbable)
    {
        if (spawnedItems.Count > 0)
        {
            spawnedItems.Remove(grabbable);
            Destroy(grabbable);
        }
    }

    public virtual GameObject GetNextItem()
    {
        if (spawnedItems.Count > 0)
        {
            GameObject nextItem = spawnedItems[0];
            return nextItem;
        }
        return null;
    }
}
