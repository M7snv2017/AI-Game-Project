using UnityEngine;

public class NavigationNode
{
    public Vector3Int position;
    public float gCost;
    public float hCost;
    public float fCost => gCost + hCost;
    public NavigationNode parent;

    public NavigationNode(Vector3Int pos)
    {
        position = pos;
    }
}
