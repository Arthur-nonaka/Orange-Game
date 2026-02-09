using UnityEngine;

public enum DiscoveryType
{
    Sprint,
    Cat,
}

public class Discoveries : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public CatManager catManager;
    public Spawner orangeSpawner;

    public bool[] unlockedDiscoveries = new bool[
        System.Enum.GetNames(typeof(DiscoveryType)).Length
    ];

    public void UnlockDiscovery(DiscoveryType discovery)
    {
        int index = (int)discovery;
        if (!unlockedDiscoveries[index])
        {
            unlockedDiscoveries[index] = true;
            ApplyDiscoveryEffects(discovery);
        }
    }

    private void ApplyDiscoveryEffects(DiscoveryType discovery)
    {
        switch (discovery)
        {
            case DiscoveryType.Sprint:
                playerMovement.sprintUnlocked = true;
                break;
            case DiscoveryType.Cat:
                catManager.SpawnGathererCat(orangeSpawner);
                break;
        }
    }

    public bool IsDiscoveryUnlocked(DiscoveryType discovery)
    {
        return unlockedDiscoveries[(int)discovery];
    }
}
