using UnityEngine;

public class OrangeSpawner : MonoBehaviour
{
    public Item Orange;
    public float spawnInterval = 5f;
    public int maxOranges;
    public Transform spawnPoint;
    public float raycastDistance = 10f;

    private int currentOranges;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && currentOranges < maxOranges)
        {
            SpawnOrange();
            timer = 0f;
        }
    }

    private void SpawnOrange()
    {
        Vector3 randomDirection = Random.onUnitSphere;
        Vector3 rayOrigin = spawnPoint.position + randomDirection * raycastDistance;

        if (Physics.Raycast(rayOrigin, -randomDirection, out RaycastHit hit, raycastDistance * 2))
        {
            GameObject orangeObject = Instantiate(Orange.prefab, hit.point, Quaternion.identity);

            Grabbable grabbable = orangeObject.GetComponent<Grabbable>();
            if (grabbable != null)
            {
                grabbable.spawner = this;
            }

            currentOranges++;
        }
    }

    public void OrangeCollected()
    {
        currentOranges = Mathf.Max(0, currentOranges - 1);
    }
}
