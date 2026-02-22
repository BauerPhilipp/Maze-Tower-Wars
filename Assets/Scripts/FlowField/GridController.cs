using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridController : MonoBehaviour
{

    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField curFlowField;

    private void InitializeFlowField()
    {
        curFlowField = new FlowField(cellRadius, gridSize);
        curFlowField.CreateGrid();
    }

    private void Start()
    {
        InitializeFlowField();
    }

    public void OnMouseClick(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Mouse Clicked");

            InitializeFlowField();

            curFlowField.CreateCostField();

            Vector3 mousePos = new Vector3(
                Mouse.current.position.ReadValue().x,
                Mouse.current.position.ReadValue().y,
                10f
            );
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Cell destinationCell = curFlowField.GetCellFromWorldPos(worldMousePos);
            curFlowField.CreateIntegrationField(destinationCell);  //TODO: hier in alt

            curFlowField.CreateFlowField();

        } 
    }
}
