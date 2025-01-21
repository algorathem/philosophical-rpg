using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeCell mazeCellPrefab;
    [SerializeField] private int mazeWidth;
    [SerializeField] private int mazeDepth;

    private MazeCell[,] mazeGrid;
    private Pathfinding pathfinding;
    private bool isMazeGenerated = false;
    private MazeCell startCell;
    private MazeCell goalCell;

    void Start()
    {
        mazeGrid = new MazeCell[mazeWidth, mazeDepth];

        // Instantiate maze cells
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeDepth; z++)
            {
                mazeGrid[x, z] = Instantiate(mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        // Generate the maze after cells are created
        GenerateMaze(null, mazeGrid[0, 0]);

        // Initialize pathfinding
        pathfinding = new Pathfinding(mazeGrid, mazeWidth, mazeDepth);

        // Set start and goal cells
        startCell = mazeGrid[0, 0];  // Example starting point
        goalCell = mazeGrid[mazeWidth - 1, mazeDepth - 1];  // Example goal point

        // Mark maze as generated
        isMazeGenerated = true;
    }

    void Update()
    {
        // Wait for 'P' key press to start pathfinding
        if (isMazeGenerated && Input.GetKeyDown(KeyCode.P) && startCell != null && goalCell != null)
        {
            List<MazeCell> path = pathfinding.FindPath(startCell, goalCell);

            if (path != null)
            {
                foreach (MazeCell cell in path)
                {
                    // Change the color of the path cells
                    cell.GetComponent<Renderer>().material.color = Color.green;
                }
            }
            else
            {
                Debug.Log("No path found!");
            }

            // Reset flag to prevent multiple pathfinding starts
            isMazeGenerated = false;
        }
    }

    void OnDrawGizmos()
    {
        if (mazeGrid == null) return;

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeDepth; z++)
            {
                Gizmos.DrawWireCube(new Vector3(x, 0, z), Vector3.one);
            }
        }

        // Mark the entrance
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(0, 0, 0), Vector3.one);  // Entrance (position 0, 0)

        // Mark the exit
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(mazeWidth - 1, 0, mazeDepth - 1), Vector3.one);  // Exit (position _mazeWidth-1, _mazeDepth-1)
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < mazeWidth)
        {
            var cellToRight = mazeGrid[x + 1, z];
            
            if (!cellToRight.IsVisited)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = mazeGrid[x - 1, z];

            if (!cellToLeft.IsVisited)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < mazeDepth)
        {
            var cellToFront = mazeGrid[x, z + 1];

            if (!cellToFront.IsVisited)
            {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = mazeGrid[x, z - 1];

            if (!cellToBack.IsVisited)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null) return;

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }
}
