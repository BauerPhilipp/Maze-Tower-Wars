using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 worldPos;
    public Vector2Int gridIndex;
    public byte cost = 1;
    public ushort bestCost = ushort.MaxValue;

    public bool isBlocked;

    public void IncreaseCost(int amnt)
    {
        if (cost == byte.MaxValue) { return; }
        if (amnt + cost >= 255) { cost = byte.MaxValue; }
        else { cost += (byte)amnt; }
    }
}
