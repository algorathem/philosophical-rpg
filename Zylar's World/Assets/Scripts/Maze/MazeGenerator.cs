using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class MazeGenerator : MonoBehaviour
{
    public int width = 10;  // Width of the maze
    public int height = 10; // Height of the maze
    public GameObject wallPrefab; // Wall prefab for maze walls
    public GameObject floorPrefab; // Floor prefab for maze floor
    public GameObject playerPrefab; // Player prefab to spawn in the maze
    public CinemachineFreeLook freeLookCamera;

    private int[,] maze;
    private Vector3 startPosition;

    void Start()
    {
        maze = new int[width, height];
        GenerateMaze();
        CreateMaze();
        SpawnPlayer();
    }

    void GenerateMaze()
    {
        // Initialize maze with all walls (1 for wall, 0 for empty space)
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = 1;
            }
        }

        // Start from a random position (usually (1,1))
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Vector2Int current = new Vector2Int(1, 1);
        maze[current.x, current.y] = 0;

        stack.Push(current);

        while (stack.Count > 0)
        {
            current = stack.Peek();
            List<Vector2Int> neighbors = GetUnvisitedNeighbors(current);
            
            if (neighbors.Count > 0)
            {
                // Choose a random neighbor and remove the wall between the current and neighbor
                Vector2Int next = neighbors[Random.Range(0, neighbors.Count)];
                maze[next.x, next.y] = 0;
                maze[(current.x + next.x) / 2, (current.y + next.y) / 2] = 0;
                stack.Push(next);
            }
            else
            {
                stack.Pop();
            }
        }
    }

    List<Vector2Int> GetUnvisitedNeighbors(Vector2Int current)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // Check all 4 neighboring cells (up, down, left, right)
        Vector2Int[] directions = {
            new Vector2Int(0, 2), // up
            new Vector2Int(0, -2), // down
            new Vector2Int(-2, 0), // left
            new Vector2Int(2, 0)  // right
        };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighbor = current + dir;
            if (neighbor.x >= 0 && neighbor.x < width && neighbor.y >= 0 && neighbor.y < height && maze[neighbor.x, neighbor.y] == 1)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    void CreateMaze()
    {
        // Instantiate walls and floors based on the maze array
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * 2, 0, y * 2);
                if (maze[x, y] == 1)
                {
                    Instantiate(wallPrefab, position, Quaternion.identity);
                }
                else
                {
                    Instantiate(floorPrefab, position, Quaternion.identity);
                }
            }
        }
    }

    void SpawnPlayer()
    {
        // Spawn the player at the start position
        Vector3 playerStartPos = new Vector3(1 * 2, 1, 1 * 2);  // Example start position (1,1)
        GameObject player = Instantiate(playerPrefab, playerStartPos, Quaternion.identity);
        if (freeLookCamera != null)
        {
            freeLookCamera.Follow = player.transform;
            freeLookCamera.LookAt = player.transform;
        }
    }
}
