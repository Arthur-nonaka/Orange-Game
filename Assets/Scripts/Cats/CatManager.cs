using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    public GathererCat gathererCatPrefab;
    public Crate cratePrefab;

    public void SpawnGathererCat(Spawner spawner)
    {
        GathererCat cat = Instantiate(
            gathererCatPrefab,
            spawner.CatSpawn.transform.position,
            spawner.CatSpawn.transform.rotation
        );

        Crate crate = Instantiate(
            cratePrefab,
            spawner.CrateSpawn.transform.position,
            spawner.CrateSpawn.transform.rotation
        );

        cat.spawner = spawner;
        cat.crate = crate;
    }
}
