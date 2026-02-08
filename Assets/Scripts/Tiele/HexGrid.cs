using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public int width = 3;
    public int height = 3;
    public float hexSize = 1f;
    public HexTile hexPrefab;

    public Dictionary<Vector2Int, HexTile> tiles = new();

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int w = 1; w < width; w++)
        {
            for (int h = 1; h < height; h++)
            {
                
                var spawnPos = new Vector3(w + (h % 2 == 0 ? 1 : 0), 0, h);
                HexTile tile = Instantiate(hexPrefab, spawnPos, Quaternion.identity, transform);
                tile.axial = new Vector2Int(w, h);
                tiles[tile.axial] = tile;
            }
        }
    }
}

