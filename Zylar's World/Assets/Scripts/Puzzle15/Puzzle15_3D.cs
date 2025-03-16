using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class Puzzle15_3D : MonoBehaviour
{
    public NumberBox boxPrefab;                   // The puzzle as a whole
    public NumberBox[,] boxes = new NumberBox[4, 4];
    public Sprite[] sprites;
    public int shuffleCount;

    public CinemachineVirtualCamera puzzleCamera;  // Assign in Inspector
    public Transform player;                       // Assign player transform
    public float interactionDistance = 3f;         // Distance to trigger interaction

    private float tileSize = 1.5f; // Adjust for 3D positioning
    private bool isPuzzleActive = false;
    private Vector3 originalPlayerPosition;
    private Vector2 offset = new Vector2(0, 0f);

    void Start()
    {
        if (sprites.Length > 0)
        {
            tileSize = 1.4f; // Adjust for better 3D spacing
        }

        Init();

        for (int i = 0; i < shuffleCount; i++)
        {
            Shuffle();
        }
    }

    void Update()
    {
        HandleInteraction();
    }

    void HandleInteraction()
    {
        // Check distance between player and puzzle
        float distance = Vector3.Distance(player.position, transform.position);

        // Press 'F' to zoom in and start the puzzle
        if (distance <= interactionDistance && Input.GetKeyDown(KeyCode.F))
        {
            print("Puzzle Interacted");
            if (!isPuzzleActive)
            {
                EnterPuzzleMode();
            }
            else
            {
                ExitPuzzleMode();
            }
        }
    }

    void EnterPuzzleMode()
    {
        isPuzzleActive = true;

        // Store player's original position to reset later
        originalPlayerPosition = player.position;

        // Enable puzzle camera
        puzzleCamera.Priority = 20;  // Higher priority than main camera

        // Disable player movement (optional)
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
        }
    }

    void ExitPuzzleMode()
    {
        isPuzzleActive = false;

        // Restore player movement
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = true;
        }

        // Reset camera priority to default
        puzzleCamera.Priority = 5;  // Lower than main camera
    }

    void Init()
    {
        int n = 0;
        for (int y = 3; y >= 0; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                Vector3 position = new Vector3(Mathf.Round(x * tileSize), 0, Mathf.Round(y * tileSize)) + transform.position;
                NumberBox box = Instantiate(boxPrefab, position, Quaternion.identity, transform); // Parent to the main object
                box.Init(x, y, offset, n + 1, sprites[n], ClickToSwap);
                boxes[x, y] = box;
                n++;
            }
        }
    }

    void ClickToSwap(int x, int y)
    {
        int dx = GetDx(x, y);
        int dy = GetDy(x, y);
        Swap(x, y, dx, dy);
    }

    void Swap(int x, int y, int dx, int dy)
    {
        var prevbox = boxes[x, y];
        var targetbox = boxes[x + dx, y + dy];

        boxes[x, y] = targetbox;
        boxes[x + dx, y + dy] = prevbox;

        prevbox.UpdatePos(x + dx, y + dy, tileSize, tileSize, offset);
        targetbox.UpdatePos(x, y, tileSize, tileSize, offset);
        if (IsPuzzleSolved())
        {
            Debug.Log("Puzzle Solved!");
            OnPuzzleCompleted();
        }
    }
    bool IsPuzzleSolved()
    {
        int correctIndex = 1; // Start from 1 because 0 is blank
        for (int y = 3; y >= 0; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                if (x == 3 && y == 0) // Last tile should be empty
                {
                    if (!boxes[x, y].IsEmpty()) return false;
                }
                else
                {
                    if (boxes[x, y].index != correctIndex) return false;
                    correctIndex++;
                }
            }
        }
        return true; // If loop completes, puzzle is solved
    }
    
    void OnPuzzleCompleted()
    {
        Debug.Log("Puzzle Complete! Do Action");
        ExitPuzzleMode();
    }

    int GetDx(int x, int y)
    {
        if (x < 3 && boxes[x + 1, y].IsEmpty()) return 1;
        if (x > 0 && boxes[x - 1, y].IsEmpty()) return -1;
        return 0;
    }

    int GetDy(int x, int y)
    {
        if (y < 3 && boxes[x, y + 1].IsEmpty()) return 1;
        if (y > 0 && boxes[x, y - 1].IsEmpty()) return -1;
        return 0;
    }

    void Shuffle()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (boxes[i, j].IsEmpty())
                {
                    Vector2 pos = getValidMove(i, j);
                    Swap(i, j, (int)pos.x, (int)pos.y);
                }
            }
        }
    }

    private Vector2 lastMove;

    Vector2 getValidMove(int x, int y)
    {
        Vector2 pos = new Vector2();
        do
        {
            int n = UnityEngine.Random.Range(0, 4);
            switch (n)
            {
                case 0: pos = Vector2.left; break;
                case 1: pos = Vector2.right; break;
                case 2: pos = Vector2.up; break;
                default: pos = Vector2.down; break;
            }
        } while (!(IsValidRange(x + (int)pos.x) && IsValidRange(y + (int)pos.y)) || IsRepeatMove(pos));

        lastMove = pos;
        return pos;
    }

    bool IsValidRange(int n)
    {
        return n >= 0 && n <= 3;
    }

    bool IsRepeatMove(Vector2 pos)
    {
        return pos * -1 == lastMove;
    }
}
