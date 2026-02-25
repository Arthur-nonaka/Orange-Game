using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    public GathererCat gathererCatPrefab;
    public Crate cratePrefab;

    public void SpawnGathererCat(Spawner spawner)
    {
        Vector3 initialPosition =
            spawner.CatInitialSpawn != null
                ? spawner.CatInitialSpawn.transform.position
                : spawner.CatSpawn.transform.position;

        GathererCat cat = Instantiate(
            gathererCatPrefab,
            initialPosition,
            spawner.CatSpawn.transform.rotation
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
            spawner.fence.Open(() => cat.WalkToPosition(spawner.CatSpawn.transform.position));
        }
        else if (spawner.CatInitialSpawn != null)
        {
            cat.WalkToPosition(spawner.CatSpawn.transform.position);
        }
    }
}
