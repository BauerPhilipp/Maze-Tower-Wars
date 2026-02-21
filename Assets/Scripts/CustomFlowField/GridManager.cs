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
    private Transform tilePrefab;
    [SerializeField]
    Vector2Int gridSize;

    VisualElement root;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coordinatesVisibleOldValue = coordinatesVisible;
        directionsVisibleOldValue = directionsVisible;
        GenerateGrid();
    }

    private void Update()
    {
        UpdateTileUI();
    }

    private void UpdateTileUI()
    {
        if (coordinatesVisible != coordinatesVisibleOldValue)
        {
            coordinatesVisibleOldValue = coordinatesVisible;
            root.Q<Label>("TileCoordinates").style.display = coordinatesVisible ? DisplayStyle.Flex : DisplayStyle.None;
        }
        //if (directionsVisible != directionsVisibleOldValue)
        //{
        //    directionsVisibleOldValue = directionsVisible;
        //    root.Q<Label>("TileDirections").style.display = directionsVisible ? DisplayStyle.Flex : DisplayStyle.None;
        //}
    }
    private void GenerateGrid()
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                var tile = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity, this.transform);
                root = tile.Find("TileUI").GetComponent<UIDocument>().rootVisualElement;
                root.Q<Label>("TileCoordinates").text = $"({x} | {y})";
            }
        }
    }
}

