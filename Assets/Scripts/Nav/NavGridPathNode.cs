using UnityEngine;

public class NavGridPathNode
{
    public int x, z;
    /// <summary>
    /// World position of the node
    /// </summary>
    public Vector3 _position;
    public bool _traversable;

    // Pathfinding Variables
    public int gCost, hCost, fCost;
    public NavGridPathNode _previousNode;

    public void CalculateFCost ( )
    {
        fCost = gCost + hCost;
    }


}