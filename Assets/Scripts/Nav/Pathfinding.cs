using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private List<NavGridPathNode> openList;
    private List<NavGridPathNode> closedList;

    // Use ASTAR algorithm to find a short path from start to end
    public List<NavGridPathNode> FindPathASTAR ( NavGridPathNode start, NavGridPathNode end )
    {
        openList = new List<NavGridPathNode> { start };
        closedList = new List<NavGridPathNode>( );

        NavGrid.m.InitNodes( );
        start.gCost = 0;
        start.hCost = NavGrid.m.CalculateDistanceCost( start.x, start.z, end.x, end.z );
        start.CalculateFCost( );

        while ( openList.Count > 0 ) {
            NavGridPathNode current = NavGrid.m.GetLowestFCostNode( openList );

            // We found a path!
            if ( current == end ) {
                //Debug.LogError( "REACHED THE END!" );
                return SimpleSmooth( CalculatePath( end ) );
            }

            // Not the end point yet!
            openList.Remove( current );
            closedList.Add( current );
            List<NavGridPathNode> neighbors = NavGrid.m.GetNeighborList( current );
            // Look at each neighbor
            foreach ( NavGridPathNode neighbor in neighbors ) {
                // Is the neighbor already on the list?
                if ( closedList.Contains( neighbor ) ) continue;
                if ( !neighbor._traversable ) {
                    closedList.Add( neighbor );
                    continue;
                }

                int tentativeGCost = current.gCost +
                                     NavGrid.m.CalculateDistanceCost( current.x, current.z, neighbor.x, neighbor.z );
                if ( tentativeGCost < neighbor.gCost ) {
                    neighbor._previousNode = current;
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = NavGrid.m.CalculateDistanceCost( neighbor.x, neighbor.z, end.x, end.z );
                    neighbor.CalculateFCost( );

                    if ( !openList.Contains( neighbor ) ) {
                        openList.Add( neighbor );
                    }
                }
            }
        }

        // Completed pathfinding, without success
        // Out of nodes on the openList
        Debug.LogError( "NO PATH FOUND!" );
        return null;
    }

    // Reverse through the path and create the final calculated path
    public List<NavGridPathNode> CalculatePath ( NavGridPathNode end )
    {
        List <NavGridPathNode> calculatedPath = new List<NavGridPathNode>();
        calculatedPath.Add( end );
        NavGridPathNode current = end;
        while ( current._previousNode != null ) {
            current = current._previousNode;
            calculatedPath.Add( current );
        }
        calculatedPath.Reverse( );
        return calculatedPath;
    }

    // Smooth the path using line of sight calculations
    public List<NavGridPathNode> SimpleSmooth ( List<NavGridPathNode> path )
    {
        int k = 0;
        List <NavGridPathNode> smoothedPath = new List<NavGridPathNode>( );
        smoothedPath.Add( path[ 0 ] );
        for ( int i = 1; i < path.Count - 1; i++ ) {
            if ( NavGrid.m.LineOfSight( smoothedPath[ k ], path[ i + 1 ] ) ) { continue; }
            smoothedPath.Add( path[ i ] );
            k++;
        }
        smoothedPath.Add( path[ path.Count - 1 ] );
        return smoothedPath;
    }


}
