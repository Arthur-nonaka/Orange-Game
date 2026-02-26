using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainStateBackup
{
    public string upgradeName;
    public Vector3 position;
    public float radius;

    // Alphamap backup
    public int alphamapStartX;
    public int alphamapStartZ;
    public float[,,] alphamapBackup;

    // Detail map backups
    public int detailStartX;
    public int detailStartZ;
    public List<int[,]> detailLayerBackups = new List<int[,]>();
}

public class TerrainTexturePainter : MonoBehaviour
{
    public Terrain terrain;
    public int textureIndex = 1;
    public float brushSize = 5f;
    public float brushStrength = 1f;

    private Dictionary<string, TerrainStateBackup> terrainBackups =
        new Dictionary<string, TerrainStateBackup>();

    public void PaintTextureAtPosition(Vector3 worldPos, int index, float radius)
    {
        int originalIndex = textureIndex;
        float originalBrushSize = brushSize;

        textureIndex = index;
        brushSize = radius;

        PaintTexture(worldPos);
        ChangeDetail(worldPos);

        textureIndex = originalIndex;
        brushSize = originalBrushSize;
    }

    public void ChangeDetail(Vector3 worldPos)
    {
        TerrainData terrainData = terrain.terrainData;

        Vector3 terrainPos = worldPos - terrain.transform.position;
        Vector3 normalizedPos = new Vector3(
            terrainPos.x / terrainData.size.x,
            0,
            terrainPos.z / terrainData.size.z
        );

        int detailMapSize = terrainData.detailWidth;
        int centerX = Mathf.FloorToInt(normalizedPos.x * detailMapSize);
        int centerZ = Mathf.FloorToInt(normalizedPos.z * detailMapSize);

        int brushRadius = Mathf.FloorToInt(brushSize * 2 / terrainData.size.x * detailMapSize);

        int startX = Mathf.Clamp(centerX - brushRadius, 0, detailMapSize);
        int startZ = Mathf.Clamp(centerZ - brushRadius, 0, detailMapSize);
        int width = Mathf.Clamp(brushRadius * 2, 1, detailMapSize - startX);
        int height = Mathf.Clamp(brushRadius * 2, 1, detailMapSize - startZ);

        for (int layer = 0; layer < terrainData.detailPrototypes.Length; layer++)
        {
            int[,] detailMap = terrainData.GetDetailLayer(startX, startZ, width, height, layer);

            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    float distanceFromCenter = Vector2.Distance(
                        new Vector2(x, z),
                        new Vector2(centerX - startX, centerZ - startZ)
                    );

                    if (distanceFromCenter <= brushRadius)
                    {
                        detailMap[z, x] = 0;
                    }
                }
            }

            terrainData.SetDetailLayer(startX, startZ, layer, detailMap);
        }
    }

    void PaintTexture(Vector3 worldPos)
    {
        TerrainData terrainData = terrain.terrainData;

        Vector3 terrainPos = worldPos - terrain.transform.position;
        Vector3 normalizedPos = new Vector3(
            terrainPos.x / terrainData.size.x,
            0,
            terrainPos.z / terrainData.size.z
        );

        int x = Mathf.FloorToInt(normalizedPos.x * terrainData.alphamapWidth);
        int z = Mathf.FloorToInt(normalizedPos.z * terrainData.alphamapHeight);

        int brushRadiusX = Mathf.FloorToInt(
            brushSize / terrainData.size.x * terrainData.alphamapWidth
        );
        int brushRadiusZ = Mathf.FloorToInt(
            brushSize / terrainData.size.z * terrainData.alphamapHeight
        );

        int mapX = Mathf.Clamp(x - brushRadiusX, 0, terrainData.alphamapWidth - 1);
        int mapZ = Mathf.Clamp(z - brushRadiusZ, 0, terrainData.alphamapHeight - 1);
        int sizeX = Mathf.Clamp(brushRadiusX * 2, 1, terrainData.alphamapWidth - mapX);
        int sizeZ = Mathf.Clamp(brushRadiusZ * 2, 1, terrainData.alphamapHeight - mapZ);

        float[,,] alphamap = terrainData.GetAlphamaps(mapX, mapZ, sizeX, sizeZ);

        for (int i = 0; i < sizeZ; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                float distanceFromCenter = Vector2.Distance(
                    new Vector2(j, i),
                    new Vector2(x - mapX, z - mapZ)
                );

                if (distanceFromCenter <= brushRadiusX)
                {
                    float strength = brushStrength;

                    float falloff = 1f - (distanceFromCenter / brushRadiusX);
                    strength *= falloff;

                    alphamap[i, j, textureIndex] = Mathf.Clamp01(
                        alphamap[i, j, textureIndex] + strength
                    );

                    float totalWeight = 0;
                    for (int l = 0; l < terrainData.alphamapLayers; l++)
                    {
                        totalWeight += alphamap[i, j, l];
                    }

                    if (totalWeight > 0)
                    {
                        for (int l = 0; l < terrainData.alphamapLayers; l++)
                        {
                            alphamap[i, j, l] /= totalWeight;
                        }
                    }
                }
            }
        }

        terrainData.SetAlphamaps(mapX, mapZ, alphamap);
    }

    public void RestoreTerrainState(string upgradeName)
    {
        if (!terrainBackups.ContainsKey(upgradeName))
        {
            Debug.LogWarning($"No backup found for {upgradeName}");
            return;
        }

        TerrainStateBackup backup = terrainBackups[upgradeName];
        TerrainData terrainData = terrain.terrainData;

        terrainData.SetAlphamaps(
            backup.alphamapStartX,
            backup.alphamapStartZ,
            backup.alphamapBackup
        );

        for (int layer = 0; layer < backup.detailLayerBackups.Count; layer++)
        {
            terrainData.SetDetailLayer(
                backup.detailStartX,
                backup.detailStartZ,
                layer,
                backup.detailLayerBackups[layer]
            );
        }
    }

    public void SaveTerrainState(string upgradeName, Vector3 worldPos, float radius)
    {
        if (terrainBackups.ContainsKey(upgradeName))
            return;

        TerrainData terrainData = terrain.terrainData;
        TerrainStateBackup backup = new TerrainStateBackup();
        backup.upgradeName = upgradeName;
        backup.position = worldPos;
        backup.radius = radius;

        Vector3 terrainPos = worldPos - terrain.transform.position;
        Vector3 normalizedPos = new Vector3(
            terrainPos.x / terrainData.size.x,
            0,
            terrainPos.z / terrainData.size.z
        );

        int x = Mathf.FloorToInt(normalizedPos.x * terrainData.alphamapWidth);
        int z = Mathf.FloorToInt(normalizedPos.z * terrainData.alphamapHeight);
        int brushRadiusX = Mathf.FloorToInt(
            radius / terrainData.size.x * terrainData.alphamapWidth
        );
        int brushRadiusZ = Mathf.FloorToInt(
            radius / terrainData.size.z * terrainData.alphamapHeight
        );

        backup.alphamapStartX = Mathf.Clamp(x - brushRadiusX, 0, terrainData.alphamapWidth - 1);
        backup.alphamapStartZ = Mathf.Clamp(z - brushRadiusZ, 0, terrainData.alphamapHeight - 1);
        int sizeX = Mathf.Clamp(
            brushRadiusX * 2,
            1,
            terrainData.alphamapWidth - backup.alphamapStartX
        );
        int sizeZ = Mathf.Clamp(
            brushRadiusZ * 2,
            1,
            terrainData.alphamapHeight - backup.alphamapStartZ
        );

        backup.alphamapBackup = terrainData.GetAlphamaps(
            backup.alphamapStartX,
            backup.alphamapStartZ,
            sizeX,
            sizeZ
        );

        int detailMapSize = terrainData.detailWidth;
        int centerX = Mathf.FloorToInt(normalizedPos.x * detailMapSize);
        int centerZ = Mathf.FloorToInt(normalizedPos.z * detailMapSize);
        int detailBrushRadius = Mathf.FloorToInt((radius * 2) / terrainData.size.x * detailMapSize);

        backup.detailStartX = Mathf.Clamp(centerX - detailBrushRadius, 0, detailMapSize);
        backup.detailStartZ = Mathf.Clamp(centerZ - detailBrushRadius, 0, detailMapSize);
        int detailWidth = Mathf.Clamp(
            detailBrushRadius * 2,
            1,
            detailMapSize - backup.detailStartX
        );
        int detailHeight = Mathf.Clamp(
            detailBrushRadius * 2,
            1,
            detailMapSize - backup.detailStartZ
        );

        for (int layer = 0; layer < terrainData.detailPrototypes.Length; layer++)
        {
            int[,] detailMap = terrainData.GetDetailLayer(
                backup.detailStartX,
                backup.detailStartZ,
                detailWidth,
                detailHeight,
                layer
            );
            backup.detailLayerBackups.Add((int[,])detailMap.Clone());
        }

        terrainBackups[upgradeName] = backup;
    }
}
