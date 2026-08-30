using UnityEngine;
public enum TileType
{
    Walkable,
    Blocked,
    Hazard
}

public class Tile : MonoBehaviour
{
    public TileType tileType;
    public bool canStandOn;
}