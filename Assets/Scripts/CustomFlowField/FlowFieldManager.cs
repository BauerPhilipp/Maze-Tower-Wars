using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlowFieldManager
{
    public Tile[,] tiles;
    public Vector2Int gridSize { get; private set; }
    public Tile destinationTile;
    public float cellRadius { get; private set; } = 1f;

    private Vector2Int[] Directions = new Vector2Int[]
    {
        new Vector2Int(0, 1), //up
        new Vector2Int(1, 0), //right
        new Vector2Int(0, -1), //down
        new Vector2Int(-1, 0) //left
    };

    public FlowFieldManager(Tile[,] tiles)
    {
        this.tiles = tiles;
    }

    public void CreateCostField()
    {
        Vector3 cellHalfExtents = Vector3.one * cellRadius;
        int terrainMask = LayerMask.GetMask("Impassible", "RoughTerrain");
        foreach (Tile curTile in tiles)
        {
            Collider[] obstacles = Physics.OverlapBox(curTile.worldPos, cellHalfExtents, Quaternion.identity, terrainMask);
            bool hasIncreasedCost = false;
            foreach (Collider col in obstacles)
            {
                if (col.gameObject.layer == 8)
                {
                    curTile.IncreaseCost(255);
                    continue;
                }
                else if (!hasIncreasedCost && col.gameObject.layer == 9)
                {
                    curTile.IncreaseCost(3);
                    hasIncreasedCost = true;
                }
            }
        }
    }

    public void CreateIntegrationField(Tile targetTile)
    {
        Queue<Tile> queue = new Queue<Tile>();

        targetTile.bestCost = 0;

        queue.Enqueue(targetTile);

        while (queue.Count > 0)
        {
            Tile curTile = queue.Dequeue();

            foreach (var dir in Directions)
            {
                Tile neighbor = GetTileAtRelativePos(curTile.gridIndex, dir);

                if (neighbor is null || neighbor.isBlocked)
                    continue;

                int newCost = curTile.bestCost + neighbor.cost;

                if(newCost < neighbor.bestCost)
                {
                    neighbor.bestCost = (ushort)newCost;
                    queue.Enqueue(neighbor);
                }

            }
            
        }
    }

    public void CreateFlowField()
    {
        foreach (Tile curTile in tiles)
        {

        }
    }

    private Tile GetTileAtRelativePos(Vector2Int orignPos, Vector2Int relativePos)
    {
        Vector2Int finalPos = orignPos + relativePos;

        if (finalPos.x < 0 || finalPos.x >= gridSize.x || finalPos.y < 0 || finalPos.y >= gridSize.y)
        {
            return null;
        }

        return tiles[finalPos.x, finalPos.y];
    }
}
