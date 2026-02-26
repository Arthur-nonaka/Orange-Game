using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    public GathererCat gathererCatPrefab;
    public Crate cratePrefab;

    public void SpawnGathererCat(Spawner spawner)
    {
        Transform initialPosition =
            spawner.CatInitialSpawn != null
                ? spawner.CatInitialSpawn.transform
                : spawner.CatSpawn.transform;

        GathererCat cat = Instantiate(
            gathererCatPrefab,
            initialPosition.position,
            initialPosition.rotation
        );

        Crate crate = Instantiate(
            cratePrefab,
            spawner.CrateSpawn.transform.position,
            spawner.CrateSpawn.transform.rotation
        );

        cat.spawner = spawner;
        cat.crate = crate;

        if (spawner.CatInitialSpawn != null && spawner.fence != null)
        {
            spawner.fence.Open(() =>
                cat.WalkToPosition(
                    spawner.CatSpawn.transform.position,
                    spawner.CatSpawn.transform.rotation,
                    spawner.fence.Close
                )
            );
        }
        else if (spawner.CatInitialSpawn != null)
        {
            cat.WalkToPosition(
                spawner.CatSpawn.transform.position,
                spawner.CatSpawn.transform.rotation,
                spawner.fence.Close
            );
        }
    }
}
