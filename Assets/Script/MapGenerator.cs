using System;
using UnityEngine;

public enum RowType
{
    Grass,
    //Road,
    River
}

[RequireComponent(typeof(GridMap))]
public class MapGenerator : MonoBehaviour
{
    public int mapWidth = 11;
    public int mapLength = 30;

    public GameObject grassPrefab;
    public GameObject waterPrefab;
    public GameObject treePrefab;

    [Header("River Moving Blocks")]
    [SerializeField] private GameObject[] riverGeneratorPrefabs;
    [SerializeField] private float generatorPadding = 2f;

    [Header("Map Data")]
    [SerializeField] private GridMap gridMap;

    private void Awake()
    {
        if (gridMap == null)
        {
            gridMap = GetComponent<GridMap>();
        }

        if (gridMap == null)
        {
            gridMap = gameObject.AddComponent<GridMap>();
        }
    }

    void Start()
    {
        gridMap.Clear();
        GenerateMap();
        gridMap.MarkReady();
    }

    private void GenerateMap()
    {
        GenerateRow(0, RowType.Grass);
        for (int z = 1; z < mapLength; z++)
        {
            RowType rowType = GetRandomRowType();

            GenerateRow(z, rowType);
        }
    }

    private void GenerateRow(int z, RowType rowType)
    {
        switch (rowType)
    {
        case RowType.Grass:
            GenerateGrassRow(z);
            break;

        // case RowType.Road:
        //     GenerateRoadRow(z);
        //     break;

        case RowType.River:
            GenerateRiverRow(z);
            break;
    }
    }

    void GenerateRiverRow(int z)
    {
        int halfWidth = mapWidth / 2;

        for (int x = -halfWidth; x <= halfWidth; x++)
        {
            Vector3 pos = new Vector3(x, 0, z);

            GameObject obj = Instantiate(
                waterPrefab,
                pos,
                Quaternion.identity
            );

            Tile tile = obj.GetComponent<Tile>();
            tile.tileType = TileType.Hazard;

            gridMap.RegisterTile(new Vector2Int(x, z), tile);
        }

        GenerateRiverMovingBlocks(z, halfWidth);
    }

    private void GenerateRiverMovingBlocks(int z, int halfWidth)
    {
        if (riverGeneratorPrefabs == null || riverGeneratorPrefabs.Length == 0)
        {
            Debug.LogWarning("River Generator Prefab 没有设置。");
            return;
        }

        int prefabIndex = UnityEngine.Random.Range(0, riverGeneratorPrefabs.Length);
        GameObject generatorPrefab = riverGeneratorPrefabs[prefabIndex];

        bool spawnFromLeft = UnityEngine.Random.value < 0.5f;
        Vector3 moveDirection = spawnFromLeft ? Vector3.right : Vector3.left;
        float spawnX = spawnFromLeft ? -halfWidth - generatorPadding : halfWidth + generatorPadding;
        Vector3 spawnPosition = new Vector3(spawnX, 0f, z);

        GameObject generatorInstance = Instantiate(
            generatorPrefab,
            spawnPosition,
            Quaternion.identity
        );

        BlockGenerator blockGenerator = generatorInstance.GetComponent<BlockGenerator>();

        if (blockGenerator != null)
        {
            blockGenerator.SetSpawnDirection(moveDirection);
        }
        else
        {
            Debug.LogWarning(
                $"{generatorPrefab.name} does not have a BlockGenerator component.",
                generatorInstance
            );
        }
    }

    private void GenerateRoadRow(int z)
    {
        throw new NotImplementedException();
    }

    private void GenerateGrassRow(int z)
    {
        int halfWidth = mapWidth / 2;

        for (int x = -halfWidth; x <= halfWidth; x++)
        {
            Vector3 pos = new Vector3(x, 0, z);

            GameObject obj = Instantiate(
                grassPrefab,
                pos,
                Quaternion.identity
            );

            Tile tile = obj.GetComponent<Tile>();

            if (UnityEngine.Random.value < 0.2f)
            {
                Instantiate(
                    treePrefab,
                    pos ,
                    Quaternion.identity
                );

                tile.tileType = TileType.Blocked;
            }
            else
            {
                tile.tileType = TileType.Walkable;
            }

            gridMap.RegisterTile(new Vector2Int(x, z), tile);
        }

    }

    private RowType GetRandomRowType()
    {
        float value = UnityEngine.Random.value;

        if (value < 0.5f)
            return RowType.Grass;

        // if (value < 0.8f)
        //     return RowType.Road;

        return RowType.River;
    }
}
