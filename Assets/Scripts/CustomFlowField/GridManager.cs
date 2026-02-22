using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private bool coordinatesVisible = true;
    private bool coordinatesVisibleOldValue = true;
    [SerializeField]
    private bool directionsVisible = true;
    private bool directionsVisibleOldValue = true;

    [SerializeField]
    private Tile tilePrefab;
    [SerializeField]
    private Vector2Int gridSize;

    public FlowFieldManager curFlowField;


    private List<Tile> tiles;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coordinatesVisibleOldValue = coordinatesVisible;
        directionsVisibleOldValue = directionsVisible;
        GenerateGrid();
        InitializeFlowField();
    }

    private void Update()
    {
        UpdateTileUI();
    }

    private void InitializeFlowField()
    {
        curFlowField = new FlowFieldManager(tiles);
    }

    //only for debugging purposes, to update the tile UI when the visibility of coordinates or directions changes in the inspector
    private void UpdateTileUI()
    {
        if (coordinatesVisible != coordinatesVisibleOldValue)
        {
            coordinatesVisibleOldValue = coordinatesVisible;
            foreach (var tile in tiles)
            {
                var root = tile.transform.Find("TileUI").GetComponent<UIDocument>().rootVisualElement;
                root.Q<Label>("TileCoordinates").style.display = coordinatesVisible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
        //if (directionsVisible != directionsVisibleOldValue)
        //{
        //    directionsVisibleOldValue = directionsVisible;
        //    root.Q<Label>("TileDirections").style.display = directionsVisible ? DisplayStyle.Flex : DisplayStyle.None;
        //}
    }
    private void GenerateGrid()
    {
        tiles = new List<Tile>();
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                var tile = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity, this.transform);
                var root = tile.transform.Find("TileUI").GetComponent<UIDocument>().rootVisualElement;
                root.Q<Label>("TileCoordinates").text = $"({x} | {y})";
                tile.gridIndex = new Vector2Int(x, y);
                tile.worldPos = tile.transform.position;
                tiles.Add(tile);
            }
        }
    }
}

