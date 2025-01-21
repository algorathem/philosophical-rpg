using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private MazeCell[,] mazeGrid;
    private int mazeWidth;
    private int mazeDepth;
    public Pathfinding(MazeCell[,] mazeGrid, int mazeWidth, int mazeDepth)
    {
        this.mazeGrid = mazeGrid;
        this.mazeWidth = mazeWidth;
        this.mazeDepth = mazeDepth;
    }

    public List<MazeCell> FindPath(MazeCell startCell, MazeCell goalCell)
    {
        // Lists to store nodes
        List<MazeCell> openList = new List<MazeCell>();  // List of nodes to be evaluated
        HashSet<MazeCell> closedList = new HashSet<MazeCell>();  // List of nodes already evaluated

        openList.Add(startCell);

        // A* loop
        while (openList.Count > 0)
        {
            // Get the node with the lowest F cost
            MazeCell currentNode = GetNodeWithLowestF(openList);
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // If goal is reached, build the path
            if (currentNode == goalCell)
            {
                return ReconstructPath(currentNode);
            }

            foreach (MazeCell neighbor in GetNeighbors(currentNode))
            {
                if (closedList.Contains(neighbor) || neighbor.IsWall) continue;

                float tentativeGCost = currentNode.GCost + 1;  // 1 is the cost of moving to a neighboring cell
                if (!openList.Contains(neighbor) || tentativeGCost < neighbor.GCost)
                {
                    neighbor.GCost = tentativeGCost;
                    neighbor.HCost = GetManhattanDistance(neighbor, goalCell);
                    neighbor.Parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null;  // No path found
    }

    private MazeCell GetNodeWithLowestF(List<MazeCell> openList)
    {
        MazeCell lowestFNode = openList[0];
        foreach (MazeCell node in openList)
        {
            if (node.FCost < lowestFNode.FCost)
            {
                lowestFNode = node;
            }
        }
        return lowestFNode;
    }

    private List<MazeCell> GetNeighbors(MazeCell currentNode)
    {
        List<MazeCell> neighbors = new List<MazeCell>();
        int x = (int)currentNode.transform.position.x;
        int z = (int)currentNode.transform.position.z;

        // Check four possible neighbors (up, down, left, right)
        if (x + 1 < mazeWidth) neighbors.Add(mazeGrid[x + 1, z]);
        if (x - 1 >= 0) neighbors.Add(mazeGrid[x - 1, z]);
        if (z + 1 < mazeDepth) neighbors.Add(mazeGrid[x, z + 1]);
        if (z - 1 >= 0) neighbors.Add(mazeGrid[x, z - 1]);

        return neighbors;
    }

    private int GetManhattanDistance(MazeCell a, MazeCell b)
    {
        return Mathf.Abs((int)a.transform.position.x - (int)b.transform.position.x) + Mathf.Abs((int)a.transform.position.z - (int)b.transform.position.z);
    }

    private List<MazeCell> ReconstructPath(MazeCell goalCell)
    {
        List<MazeCell> path = new List<MazeCell>();
        MazeCell current = goalCell;

        while (current.Parent != null)
        {
            path.Add(current);
            current = current.Parent;
        }

        path.Reverse();  // Reverse the path to start from the start point
        return path;
    }
}
