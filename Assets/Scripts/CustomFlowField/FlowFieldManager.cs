using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlowFieldManager
{
    public List<Tile> tiles;
    public Vector2Int gridSize { get; private set; }
    public Tile destinationTile;
    public float cellRadius { get; private set; } = 1f;

    public FlowFieldManager(List<Tile> tiles)
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

    //TODO: weiter hier
    public void CreateIntegrationField(Tile _destinationTile)
    {
        destinationTile = _destinationTile;

        destinationTile.cost = 0;
        destinationTile.bestCost = 0;

        Queue<Tile> cellsToCheck = new Queue<Tile>();

        cellsToCheck.Enqueue(destinationTile);

        while (cellsToCheck.Count > 0)
        {
            Tile curCell = cellsToCheck.Dequeue();
            List<Tile> curNeighbors = GetNeighborCells(curCell.gridIndex, GridDirection.CardinalDirections);
            foreach (Tile curNeighbor in curNeighbors)
            {
                if (curNeighbor.cost == byte.MaxValue) { continue; }
                if (curNeighbor.cost + curCell.bestCost < curNeighbor.bestCost)
                {
                    curNeighbor.bestCost = (ushort)(curNeighbor.cost + curCell.bestCost);
                    cellsToCheck.Enqueue(curNeighbor);
                }
            }
        }
    }

    public void CreateFlowField()
    {
        foreach (Tile curTile in tiles)
        {
            List<Tile> curNeighbors = GetNeighborCells(curTile.gridIndex, GridDirection.AllDirections);

            int bestCost = curTile.bestCost;

            foreach (Tile curNeighbor in curNeighbors)
            {
                if (curNeighbor.bestCost < bestCost)
                {
                    bestCost = curNeighbor.bestCost;
                    curTile.bestDirection = GridDirection.GetDirectionFromV2I(curNeighbor.gridIndex - curTile.gridIndex);
                }
            }
        }
    }

    private List<Tile> GetNeighborCells(Vector2Int nodeIndex, List<GridDirection> directions)
    {
        List<Tile> neighborTile = new List<Tile>();

        foreach (Vector2Int curDirection in directions)
        {
            Tile newNeighbor = GetCellAtRelativePos(nodeIndex, curDirection);
            if (newNeighbor != null)
            {
                neighborTile.Add(newNeighbor);
            }
        }
        return neighborTile;
    }

    private Tile GetCellAtRelativePos(Vector2Int orignPos, Vector2Int relativePos)
    {
        Vector2Int finalPos = orignPos + relativePos;

        if (finalPos.x < 0 || finalPos.x >= gridSize.x || finalPos.y < 0 || finalPos.y >= gridSize.y)
        {
            return null;
        }

        else { return tiles.First(t => t.gridIndex == finalPos); }
    }

    //public Cell GetCellFromWorldPos(Vector3 worldPos)
    //{
    //    float percentX = worldPos.x / (gridSize.x * cellDiameter);
    //    float percentY = worldPos.z / (gridSize.y * cellDiameter);

    //    percentX = Mathf.Clamp01(percentX);
    //    percentY = Mathf.Clamp01(percentY);

    //    int x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
    //    int y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);
    //    return grid[x, y];
    //}
}
