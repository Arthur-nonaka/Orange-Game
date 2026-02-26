using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeTerrainChange
{
    public string upgradeName;
    public Vector3 position;
    public int textureIndex;
    public float paintRadius = 5f;

    [HideInInspector]
    public bool isApplied = false;
}

public class TerrainUpgradeManager : MonoBehaviour
{
    public static TerrainUpgradeManager Instance { get; private set; }

    public TerrainTexturePainter texturePainter;
    public List<UpgradeTerrainChange> upgradeChanges = new List<UpgradeTerrainChange>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SaveTerrainState();
    }

    private void SaveTerrainState()
    {
        foreach (var upgrade in upgradeChanges)
        {
            texturePainter.SaveTerrainState(
                upgrade.upgradeName,
                upgrade.position,
                upgrade.paintRadius
            );
            upgrade.isApplied = false;
        }
    }

    public void ResetToBasic()
    {
        foreach (var upgrade in upgradeChanges)
        {
            if (upgrade.isApplied)
            {
                texturePainter.RestoreTerrainState(upgrade.upgradeName);
                upgrade.isApplied = false;
                Debug.Log($"Reset terrain for upgrade: {upgrade.upgradeName}");
            }
        }
    }

    public void ApplyUpgrade(string upgradeName)
    {
        UpgradeTerrainChange change = upgradeChanges.Find(u => u.upgradeName == upgradeName);

        if (change != null)
        {
            texturePainter.PaintTextureAtPosition(
                change.position,
                change.textureIndex,
                change.paintRadius
            );
            change.isApplied = true;
        }
    }

    public void RevertUpgrade(string upgradeName)
    {
        UpgradeTerrainChange change = upgradeChanges.Find(u => u.upgradeName == upgradeName);

        if (change != null && change.isApplied)
        {
            texturePainter.RestoreTerrainState(upgradeName);
            change.isApplied = false;
        }
    }
}
