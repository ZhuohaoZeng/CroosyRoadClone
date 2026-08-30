// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UIElements;

// public enum RowType
// {
//     Grass,
//     Road,
//     River
// }


// public class MapManager : MonoBehaviour
// {
//     public int mapWidth = 11;
//     public int mapLength = 30;

//     public GameObject grassPrefab;
//     public GameObject waterPrefab;
//     public GameObject treePrefab;

//     private Dictionary<Vector2Int, Tile> tileMap;
//     private MapGenerator mapGenerator;
//     void Start()
//     {
//         tileMap = new Dictionary<Vector2Int, Tile>();
//         mapGenerator = new MapGenerator();
//         mapGenerator.GenerateMap(mapLength);
//     }

//     //Below are check grid Map logics
//     void tryMoveTo(Vector2Int pos)
//     {
//         Tile nextPos = tileMap[pos];
//         switch (nextPos.tileType)
//         {
//             case TileType.Walkable:
//                 break;
//             case TileType.Blocked:
//                 break;
//             case TileType.Hazard:
//                 break;
//         }
        
//     }
//     bool isWalkable(Vector2Int pos)
//     {
//         return tileMap[pos].tileType == TileType.Walkable;
//     }
// }