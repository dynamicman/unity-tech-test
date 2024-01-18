using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NavGrid : MonoBehaviour
{
    public static NavGrid m;
    [SerializeField]
    NavGridPathNode[,] _nodes;
    public const int DETAIL = 100;
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public Pathfinding _pathfinding;

    private void Awake ( )
    {
        m = this;
        GenerateTraversable( );
        _pathfinding = new Pathfinding( );
    }

    public NavGridPathNode FindNearestNode ( Vector3 position )
    {
        int width = 50;
        float start_x = -1.0f * (float)width / 2.0f;
        float detail_x = (float)width / (float)DETAIL;
        float start_z = -1.0f * (float)width / 2.0f;
        float detail_z = (float)width / (float)DETAIL;
        float testHeightHalf = 1.0f;
        float testWidthHalf = 0.5f;

        // Convert x,z position to node value
        int x = Mathf.FloorToInt( (position.x - start_x ) / detail_x );
        int z = Mathf.FloorToInt( (position.z - start_z ) / detail_z );
        Debug.Log( " click from [" + position.x + "," + position.z + "]" );
        Debug.Log( " click to [" + x + "," + z + "]" );

        if ( _nodes[ x, z ]._traversable ) {
            lastClickedNode = _nodes[ x, z ];
        } else {
            lastClickedNode = null;
        }
        return lastClickedNode;
    }

    public NavGridPathNode lastClickedNode;

    public NavGridPathNode GetNode ( int x, int z )
    {
        //TODO: Add checks
        return _nodes[ x, z ];
    }

    public void InitNodes ( )
    {
        NavGridPathNode node;
        for ( int x = 0; x < DETAIL; x++ ) {
            for ( int z = 0; z < DETAIL; z++ ) {
                node = _nodes[ x, z ];
                node.gCost = int.MaxValue;
                node.CalculateFCost( );
                node._previousNode = null;
            }
        }
    }

    public List<NavGridPathNode> GetNeighborList ( NavGridPathNode here )
    {
        List<NavGridPathNode> neighbors = new List<NavGridPathNode>();
        if ( here.x - 1 >= 0 ) {
            // Left
            neighbors.Add( GetNode( here.x - 1, here.z ) );
            // Left Down
            if ( here.z - 1 >= 0 ) neighbors.Add( GetNode( here.x - 1, here.z - 1 ) );
            // Left Up
            if ( here.z + 1 < DETAIL ) neighbors.Add( GetNode( here.x - 1, here.z + 1 ) );
        }

        if ( here.x - 1 >= 0 ) {
            // Right
            neighbors.Add( GetNode( here.x + 1, here.z ) );
            // Right Down
            if ( here.z - 1 >= 0 ) neighbors.Add( GetNode( here.x + 1, here.z - 1 ) );
            // Right Up
            if ( here.z + 1 < DETAIL ) neighbors.Add( GetNode( here.x + 1, here.z + 1 ) );
        }
        // Down
        if ( here.z - 1 >= 0 ) neighbors.Add( GetNode( here.x, here.z - 1 ) );
        // Up
        if ( here.z + 1 < DETAIL ) neighbors.Add( GetNode( here.x, here.z + 1 ) );

        return neighbors;
    }

    public int CalculateDistanceCost ( int a_x, int a_z, int b_x, int b_z )
    {
        int xDistance = Mathf.Abs( a_x - b_x );
        int zDistance = Mathf.Abs( a_z - b_z );
        int remaining = Mathf.Abs( xDistance - zDistance );
        return MOVE_DIAGONAL_COST * Mathf.Min( xDistance, zDistance ) +
               MOVE_STRAIGHT_COST * remaining;
    }

    public NavGridPathNode GetLowestFCostNode ( List<NavGridPathNode> nodeList )
    {
        NavGridPathNode lowestFCostNode = nodeList[ 0 ];
        for ( int i = 1; i < nodeList.Count; i++ ) {
            if ( nodeList[ i ].fCost < lowestFCostNode.fCost ) {
                lowestFCostNode = nodeList[ i ];
            }
        }
        return lowestFCostNode;
    }

    // Generate all path nodes and determine traversable-ness
    public void GenerateTraversable ( )
    {
        int width = 50;
        float start_x = -1.0f * (float)width / 2.0f;
        float detail_x = (float)width / (float)DETAIL;
        float start_z = -1.0f * (float)width / 2.0f;
        float detail_z = (float)width / (float)DETAIL;
        float testHeightHalf = 1.0f;
        float testWidthHalf = 0.5f;

        _nodes = new NavGridPathNode[ DETAIL, DETAIL ];

        var _obstacleLayerMask = 1 << 3;
        Vector3 halfExtents = new Vector3(testWidthHalf, testHeightHalf, testWidthHalf);


        for ( int x = 0; x < DETAIL; x++ ) {
            for ( int z = 0; z < DETAIL; z++ ) {
                _nodes[ x, z ] = new NavGridPathNode( );
                Vector3 position = new Vector3( x * detail_x + start_x, testHeightHalf, z * detail_z + start_z );

                Debug.Log( "Placing [" + x + "," + z + "] at [" + position.x + "," + position.z + "]" );
                _nodes[ x, z ]._position = position;
                _nodes[ x, z ].x = x;
                _nodes[ x, z ].z = z;
                _nodes[ x, z ]._traversable = !Physics.CheckBox( _nodes[ x, z ]._position, halfExtents, Quaternion.identity, _obstacleLayerMask );
            }
        }
    }


    private List<NavGridPathNode> RecentPath;
    /// <summary>
    /// Given the current and desired location, return a path to the destination
    /// </summary>
    public NavGridPathNode[ ] GetPath ( Vector3 origin, Vector3 destination )
    {
        Debug.Log( " will pathfind " + ( _pathfinding == null ) );
        NavGridPathNode originNode = FindNearestNode( origin );
        NavGridPathNode destinationNode = FindNearestNode( destination );
        RecentPath = _pathfinding.FindPath( originNode, destinationNode );

        if ( RecentPath != null ) {
            return RecentPath.ToArray( );
        } else {
            Debug.LogError( "Failed to get a path!" );
            return new NavGridPathNode[ ]
            {
                new() { _position = origin },
                new() { _position = destination }
            };
        } 
    }

    // Draw the traversable area in teal
    // Draw the obstacle area in red
    public void OnDrawGizmos ( )
    {
        if ( _nodes == null ) { return; }

        int detail = 100;
        Vector3 rectangleSize = new(1.0f, 0f, 1.0f);
        for ( int x = 0; x < detail; x++ ) {
            for ( int z = 0; z < detail; z++ ) {
                Gizmos.color = ( _nodes[ x, z ]._traversable ) ? new Color( 0, 1, 1, 0.25f ) : new Color( 1, 0, 0, 0.25f );
                Gizmos.DrawCube( _nodes[ x, z ]._position, rectangleSize );
                if ( lastClickedNode != null ) {
                    Gizmos.color = new Color( 1, 1, 0, 0.25f );
                    Gizmos.DrawSphere( lastClickedNode._position, 0.5f );
                }

                if ( RecentPath != null && RecentPath.Contains( _nodes[ x, z ] ) ) {
                    Gizmos.color = new Color( 0, 1, 0, 0.25f );
                    Gizmos.DrawSphere( _nodes[ x, z ]._position, 0.5f );
                }
            }
        }
    }
}
