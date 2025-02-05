using UnityEngine;
using System.Collections.Generic;
using Cinemachine;

public class FlowPuzzle : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject dotPrefab;
    public LineRenderer pathPrefab;
    public int gridSize = 6;
    public List<DotPair> dotPairsConfig;

    private Transform[,] grid;
    private Dictionary<Color, List<Vector2Int>> dotPairs;
    private Dictionary<Color, LineRenderer> paths;
    private Dictionary<Color, Stack<Vector2Int>> pathHistories;
    private Dictionary<Color, bool> pairCompleted;
    private bool[,] cellOccupied;
    private Color activeColor = Color.clear;
    private List<Vector2Int> currentPath;
    private Camera mainCamera;

    public Vector3 spawnOrigin = new Vector3(10f, 0f, 10f);
    public Material dotMaterial;

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
                Debug.Log(dotColor.ToString());
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
    }

    void GenerateGrid()
    {
        float cellSize = 1.0f;

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                GameObject cell = Instantiate(cellPrefab, transform);
                cell.transform.position = spawnOrigin + new Vector3(x * cellSize, y * cellSize, 0);
                cell.transform.rotation = Quaternion.Euler(0, 0, 0);
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
        }

        dotPairs[color].Add(pos1);
        dotPairs[color].Add(pos2);

        PlaceDot(color, pos1);
        PlaceDot(color, pos2);
    }

    void PlaceDot(Color color, Vector2Int position)
    {
        GameObject dot = Instantiate(dotPrefab, transform);
        dot.transform.position = spawnOrigin + new Vector3(position.x, position.y, 0.0f);

        MeshRenderer meshRenderer = dot.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material = dotMaterial;
            meshRenderer.material.color = color;
        }
        else
        {
            Debug.LogError("MeshRenderer missing on dot prefab! Ensure the sphere has a MeshRenderer.");
        }
    }

    void StartPath(Color dotColor, Vector2Int startCell)
    {
        if (paths.ContainsKey(dotColor))
        {
            Destroy(paths[dotColor].gameObject);
            paths.Remove(dotColor);
        }
        if (pathHistories.ContainsKey(dotColor))
        {
            // Reset all occupied cells before clearing path history
            foreach (Vector2Int cell in pathHistories[dotColor])
            {
                cellOccupied[cell.x, cell.y] = false;
            }

            pathHistories[dotColor].Clear();
            pairCompleted[dotColor] = false;
        }
        activeColor = dotColor;
        currentPath = new List<Vector2Int> { startCell };
        cellOccupied[startCell.x, startCell.y] = true;

        if (!paths.ContainsKey(dotColor))
        {
            LineRenderer newPath = Instantiate(pathPrefab, transform);
            newPath.startColor = dotColor;
            newPath.endColor = dotColor;
            newPath.positionCount = 0;
            paths[dotColor] = newPath;
        }

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
            return;

        // Allow only up, down, left, right moves
        Vector2Int lastCell = currentPath[currentPath.Count - 1];
        int deltaX = Mathf.Abs(cellPosition.x - lastCell.x);
        int deltaY = Mathf.Abs(cellPosition.y - lastCell.y);

        if (!((deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1)))
            return;

        // Check if the cell is already occupied or if the path is completed
        if (cellOccupied[cellPosition.x, cellPosition.y] && !currentPath.Contains(cellPosition))
            return;

        // Add the new cell to the path
        currentPath.Add(cellPosition);
        pathHistories[activeColor].Push(cellPosition);

        // Get the corresponding LineRenderer
        LineRenderer line = paths[activeColor];

        // Ensure the LineRenderer uses world space
        line.useWorldSpace = true;
        line.positionCount = currentPath.Count;

        for (int i = 0; i < currentPath.Count; i++)
        {
            Vector3 position = grid[currentPath[i].x, currentPath[i].y].position;
            position.z -= 0.05f; // Push it slightly forward to avoid depth overlap
            line.SetPosition(i, position);
        }

        // Mark the cell as occupied
        cellOccupied[cellPosition.x, cellPosition.y] = true;

        // Debugging to ensure the path is correctly recorded
        Debug.Log($"Path extended to: {cellPosition} in world position {grid[cellPosition.x, cellPosition.y].position}");
    }

    void EndPath()
    {
        if (currentPath.Count > 1)
        {
            List<Vector2Int> dotPositions = dotPairs[activeColor];
            if (dotPositions.Count == 2 && currentPath.Contains(dotPositions[0]) && currentPath.Contains(dotPositions[1]))
            {
                pairCompleted[activeColor] = true;
            }
        }

        activeColor = Color.clear;
        currentPath = null;
        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        foreach (var completed in pairCompleted.Values)
        {
            if (!completed)
                return;
        }

        Debug.Log("You win!");
    }

    Vector2Int ConvertWorldToGrid(Vector3 worldPosition)
    {
        Vector3 localPosition = worldPosition - spawnOrigin;
        int gridX = Mathf.RoundToInt(localPosition.x);
        int gridY = Mathf.RoundToInt(localPosition.y);

        gridX = Mathf.Clamp(gridX, 0, gridSize - 1);
        gridY = Mathf.Clamp(gridY, 0, gridSize - 1);
        
        return new Vector2Int(gridX, gridY);
    }

    Vector2Int GetCellUnderMouse()
    {
        Camera activeCam = Camera.main;
        
        Ray ray = activeCam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 2f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.collider.CompareTag("Dot"))  // If the dot is directly hit
            {
                Debug.Log($"Direct Dot hit detected: {hit.collider.gameObject.name}");
                return ConvertWorldToGrid(hit.collider.transform.position);
            }
            if (hit.collider.CompareTag("Grid"))
            {
                return ConvertWorldToGrid(hit.point);
            }
            
        }

        return -Vector2Int.one;
    }

    bool IsDot(Vector2Int cellPosition, out Color dotColor)
    {
        Debug.Log($"Checking for dot at: {cellPosition}"); // Debugging

        foreach (var pair in dotPairs)
        {
            if (pair.Value.Contains(cellPosition))
            {
                dotColor = pair.Key;
                Debug.Log($" Dot found at {cellPosition} with color: {dotColor}"); // Confirmation
                return true;
            }
        }

        dotColor = Color.clear;
        Debug.Log($" No dot found at {cellPosition}"); // Debugging
        return false;
    }
}
