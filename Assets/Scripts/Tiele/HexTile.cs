using UnityEngine;

public class HexTile : MonoBehaviour
{
    public Vector2Int axial; // (q, r)
    public bool isBlocked;

    [HideInInspector]
    public int distanceToTarget;
}
