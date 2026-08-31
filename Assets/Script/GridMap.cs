using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    private readonly Dictionary<Vector2Int, Tile> tileMap = new();

    public bool IsReady { get; private set; }

    public void Clear()
    {
        tileMap.Clear();
        IsReady = false;
    }

    public void RegisterTile(Vector2Int position, Tile tile)
    {
        if (tile == null)
        {
            Debug.LogError($"Cannot register a missing Tile at {position}.", this);
            return;
        }

        tileMap[position] = tile;
    }

    public void RemoveTile(Vector2Int position)
    {
        tileMap.Remove(position);
    }

    public bool TryGetTile(Vector2Int position, out Tile tile)
    {
        return tileMap.TryGetValue(position, out tile);
    }

    public bool TryGetTileType(Vector2Int position, out TileType tileType)
    {
        if (TryGetTile(position, out Tile tile))
        {
            tileType = tile.tileType;
            return true;
        }

        tileType = default;
        return false;
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPosition.x),
            Mathf.RoundToInt(worldPosition.z)
        );
    }

    public Vector3 GridToWorld(Vector2Int gridPosition, float worldY)
    {
        return new Vector3(gridPosition.x, worldY, gridPosition.y);
    }

    public void MarkReady()
    {
        IsReady = true;
    }
}
