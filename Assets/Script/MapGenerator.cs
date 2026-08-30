using System;
using System.Collections.Generic;
using UnityEngine;

public enum RowType
{
    Grass,
    //Road,
    River
}

public class MapGenerator : MonoBehaviour
{
    public int mapWidth = 11;
    public int mapLength = 30;

    public GameObject grassPrefab;
    public GameObject waterPrefab;
    public GameObject treePrefab;

    private Dictionary<Vector2Int, Tile> tileMap;

    void Start()
    {
        tileMap = new Dictionary<Vector2Int, Tile>();
        GenerateMap();
    }

    private void GenerateMap()
    {
        for (int z = 0; z < mapLength; z++)
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

        tileMap[new Vector2Int(x, z)] = tile;
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

            tileMap[new Vector2Int(x, z)] = tile;
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


//Below are check grid Map logics
    void tryMoveTo(Vector2Int pos)
    {
        Tile nextPos = tileMap[pos];
        switch (nextPos.tileType)
        {
            case TileType.Walkable:
                break;
            case TileType.Blocked:
                break;
            case TileType.Hazard:
                break;
        }
        
    }
    bool isWalkable(Vector2Int pos)
    {
        return tileMap[pos].tileType == TileType.Walkable;
    }
}