using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private bool coordinatesVisible;
    private bool coordinatesVisibleOldValue;
    [SerializeField]
    private bool directionsVisible = true;
    private bool directionsVisibleOldValue = true;

    [SerializeField]
    private Tile tilePrefab;
    [SerializeField]
    private Vector2Int gridSize;

    public FlowFieldManager curFlowField;


    private Tile[,] tiles;
    private Tile targetTile;


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
        tiles = new Tile[gridSize.x, gridSize.y];
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                var tile = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity, this.transform);
                var root = tile.transform.Find("TileUI").GetComponent<UIDocument>().rootVisualElement;
                root.Q<Label>("TileCoordinates").text = $"({x} | {y})";
                tile.gridIndex = new Vector2Int(x, y);
                tile.worldPos = tile.transform.position;
                tiles[x,y] = tile;
            }
        }
    }

    private void OnMouseClick(InputValue value)
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit, 1000f);

        if(hit.collider.CompareTag("Tile"))
        {
            Debug.Log("Tile Clicked");
            var clickedTile = hit.collider.GetComponent<Tile>();
            if (clickedTile != null)
            {
                curFlowField.CreateCostField();
                curFlowField.CreateIntegrationField(clickedTile);

                ChangeBorderColor(clickedTile);
                targetTile = clickedTile;

            }
        }


        Debug.Log("Mouse Clicked");
    }

    private void ChangeBorderColor(Tile tile)
    {
        if(targetTile != null)
        {
            var oldTargetBorder = targetTile.transform.Find("TileUI").GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Border");
            oldTargetBorder.style.borderBottomColor = oldTargetBorder.style.borderTopColor;
        }

        var border = tile.transform.Find("TileUI").GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Border");
        border.style.borderBottomColor = Color.red;
    }

}

