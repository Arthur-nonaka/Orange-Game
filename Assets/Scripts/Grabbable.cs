using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public Item item;
    public Spawner spawner;

    public Item GetItem()
    {
        return item;
    }

    public void NotifySpawner()
    {
        spawner.ItemCollected(this.gameObject);
    }
}
