using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public Item item;
    public OrangeSpawner spawner;

    public Item GetItem()
    {
        return item;
    }

    public void NotifySpawner()
    {
        spawner.OrangeCollected();
    }
}
