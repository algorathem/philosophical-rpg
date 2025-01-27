using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FlowPuzzle : MonoBehaviour
{
    public GameObject cellPrefab; // Prefab for each cell
    public GameObject dotPrefab; // Prefab for the colored dots
    public LineRenderer pathPrefab; // Prefab for path lines
    public int gridSize = 6; // Grid size (e.g., 6x6)
    public List<DotPair> dotPairsConfig;

    private Transform[,] grid; // 2D array to hold the grid cells
    private Dictionary<Color, List<Vector2Int>> dotPairs; // Stores positions of paired dots
    private Dictionary<Color, LineRenderer> paths; // Stores paths for each color
    private Dictionary<Color, Stack<Vector2Int>> pathHistories; // Stack to track path history
    private Dictionary<Color, bool> pairCompleted; // Tracks if each pair is completed
    private bool[,] cellOccupied; // Tracks if a cell is occupied by a path
    private Color activeColor = Color.clear; // Currently selected dot color
    private List<Vector2Int> currentPath; // Current path being drawn
    private Camera mainCamera;

    [SerializeField] private AudioClip soundBeep;
    [SerializeField] private AudioClip soundBoom;

    void Start()
    {
        mainCamera = Camera.main;
        grid = new Transform[gridSize, gridSize];
        dotPairs = new Dictionary<Color, List<Vector2Int>>();
        paths = new Dictionary<Color, LineRenderer>();
        pathHistories = new Dictionary<Color, Stack<Vector2Int>>();
        pairCompleted = new Dictionary<Color, bool>();
        cellOccupied = new bool[gridSize, gridSize];
        GenerateGrid();
        PlaceDots();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int cellPosition = GetCellUnderMouse();
            if (cellPosition != -Vector2Int.one && IsDot(cellPosition, out Color dotColor))
            {
                StartPath(dotColor, cellPosition);
            }
        }
        else if (Input.GetMouseButton(0) && activeColor != Color.clear)
        {
            Vector2Int cellPosition = GetCellUnderMouse();
            if (cellPosition != -Vector2Int.one && !currentPath.Contains(cellPosition))
            {
                ExtendPath(cellPosition);
            }
        }
        else if (Input.GetMouseButtonUp(0) && activeColor != Color.clear)
        {
            EndPath();
        }

        // Reset the game when "R" is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }

        // Exit Game when "F" is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene("BrokenStairs");
        }
    }

    void GenerateGrid()
    {
        float cellSize = 1.0f; // Size of each grid cell
        Vector2 gridOrigin = new Vector2(-gridSize / 2f, -gridSize / 2f); // Center the grid on the screen

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                // Instantiate grid cell
                GameObject cell = Instantiate(cellPrefab, transform);

                // Set the cell's position
                cell.transform.position = new Vector3(gridOrigin.x + x * cellSize, gridOrigin.y + y * cellSize, 0);

                // Optional: Scale the cell (ensure consistent size)
                cell.transform.localScale = new Vector3(cellSize, cellSize, 1);

                // Store the cell in the grid array
                grid[x, y] = cell.transform;
            }
        }
    }

    void PlaceDots()
    {
        foreach (DotPair pair in dotPairsConfig)
        {
            AddDotPair(pair.color, pair.position1, pair.position2);
        }
    }

    void AddDotPair(Color color, Vector2Int pos1, Vector2Int pos2)
    {
        if (!dotPairs.ContainsKey(color))
        {
            dotPairs[color] = new List<Vector2Int>();
            pairCompleted[color] = false;
            pathHistories[color] = new Stack<Vector2Int>();
            Debug.Log($"Adding new dot pair for color: {color}");
        }

        dotPairs[color].Add(pos1);
        dotPairs[color].Add(pos2);

        // Instantiate dots
        PlaceDot(color, pos1);
        PlaceDot(color, pos2);
    }

    void PlaceDot(Color color, Vector2Int position)
    {
        GameObject dot = Instantiate(dotPrefab, transform);
        dot.transform.position = grid[position.x, position.y].position + new Vector3(0, 0, -0.1f); ;
        dot.GetComponent<SpriteRenderer>().color = color;
    }

    void StartPath(Color dotColor, Vector2Int startCell)
    {
        if (paths.ContainsKey(dotColor))
        {
            // Clear the existing path
            LineRenderer existingPath = paths[dotColor];
            Destroy(existingPath.gameObject);
            paths.Remove(dotColor);

            // Mark all cells in the path as unoccupied
            if (pathHistories.ContainsKey(dotColor))
            {
                Stack<Vector2Int> history = pathHistories[dotColor];
                while (history.Count > 0)
                {
                    Vector2Int cell = history.Pop();
                    cellOccupied[cell.x, cell.y] = false;
                }
            }

            // Reset the completion status for this pair
            pairCompleted[dotColor] = false;
        }

        // Start a new path
        activeColor = dotColor;
        currentPath = new List<Vector2Int> { startCell };

        // Mark the starting cell as occupied
        cellOccupied[startCell.x, startCell.y] = true;

        // Create a new LineRenderer for this color
        if (!paths.ContainsKey(dotColor))
        {
            LineRenderer newPath = Instantiate(pathPrefab, transform);
            newPath.startColor = dotColor;
            newPath.endColor = dotColor;
            newPath.positionCount = 0;
            paths[dotColor] = newPath;
        }

        // Initialize the history stack for this color
        if (!pathHistories.ContainsKey(dotColor))
        {
            pathHistories[dotColor] = new Stack<Vector2Int>();
        }
        pathHistories[dotColor].Push(startCell);
    }

    void ExtendPath(Vector2Int cellPosition)
    {
        // Ensure the cell is within bounds
        if (cellPosition.x < 0 || cellPosition.x >= gridSize || cellPosition.y < 0 || cellPosition.y >= gridSize)
            return;

        // Prevent overlapping or reversing the line
        if (currentPath.Count > 1 && cellPosition == currentPath[currentPath.Count - 2])
        {
            Debug.Log("Cannot reverse the line to overlap!");
            return;
        }

        // Allow only up, down, left, right moves
        Vector2Int lastCell = currentPath[currentPath.Count - 1];
        int deltaX = Mathf.Abs(cellPosition.x - lastCell.x);
        int deltaY = Mathf.Abs(cellPosition.y - lastCell.y);

        if (!((deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1)))
        {
            Debug.Log("Only up, down, left, or right moves are allowed!");
            return;
        }

        // Check if the cell is already occupied or if the path is completed
        if (cellOccupied[cellPosition.x, cellPosition.y] && !currentPath.Contains(cellPosition))
        {
            Debug.Log("Cell is already occupied!");
            return;
        }

        // Add the new cell to the path
        currentPath.Add(cellPosition);
        pathHistories[activeColor].Push(cellPosition);

        // Update the LineRenderer
        LineRenderer line = paths[activeColor];
        line.positionCount = currentPath.Count;
        for (int i = 0; i < currentPath.Count; i++)
        {
            Vector3 position = grid[currentPath[i].x, currentPath[i].y].position;
            line.SetPosition(i, position);
        }
        SoundFXManager.instance.PlaySoundFXClip(soundBeep, transform, 1f);

        // Mark the cell as occupied
        cellOccupied[cellPosition.x, cellPosition.y] = true;
    }

    void EndPath()
    {
        if (currentPath.Count > 1)
        {
            // Check if the path connects the two dots of the same color
            List<Vector2Int> dotPositions = dotPairs[activeColor];
            if (currentPath.Contains(dotPositions[0]) && currentPath.Contains(dotPositions[1]))
            {
                // Check if the path has valid flow between the two dots (e.g., no overlapping paths)
                if (!pairCompleted[activeColor]) // Only mark as complete if it's not already
                {
                    pairCompleted[activeColor] = true; // Mark this pair as completed
                }
            }
        }

        // Reset active color and path
        activeColor = Color.clear;
        currentPath = null;

        CheckWinCondition();
    }

    void ResetGame()
    {
        // Clear all paths
        foreach (var path in paths.Values)
        {
            Destroy(path.gameObject);
        }

        // Clear paths and histories
        paths.Clear();
        pathHistories.Clear();

        // Reset tracking variables
        pairCompleted.Clear();
        cellOccupied = new bool[gridSize, gridSize];

        // Clear all existing dots
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Dot")) // Ensure you have assigned a "Dot" tag to your dot prefab
            {
                Destroy(child.gameObject);
            }
        }

        // Reinitialize dot pairs as incomplete
        dotPairs.Clear(); // Ensure this is cleared before adding the dot pairs again
        AddDotPair(Color.red, new Vector2Int(0, 0), new Vector2Int(5, 5));
        AddDotPair(Color.blue, new Vector2Int(1, 1), new Vector2Int(4, 4));
        AddDotPair(Color.green, new Vector2Int(2, 2), new Vector2Int(3, 3));

        // Reinitialize the grid
        //GenerateGrid();
    }

    void CheckWinCondition()
    {
        foreach (var completed in pairCompleted.Values)
        {
            if (!completed)
            {
                return; // Not all pairs are completed
            }
        }
        SoundFXManager.instance.PlaySoundFXClip(soundBoom, transform, 1f);

        Debug.Log("You win!");
    }

    Vector2Int GetCellUnderMouse()
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (Vector3.Distance(mouseWorldPosition, grid[x, y].position) < 0.5f)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return -Vector2Int.one; // No valid cell found
    }

    bool IsDot(Vector2Int cellPosition, out Color dotColor)
    {
        foreach (var pair in dotPairs)
        {
            if (pair.Value.Contains(cellPosition))
            {
                dotColor = pair.Key;
                return true;
            }
        }

        dotColor = Color.clear;
        return false;
    }
}
