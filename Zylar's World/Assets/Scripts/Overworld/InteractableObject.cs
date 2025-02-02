using Cinemachine;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public KeyCode interactionKey = KeyCode.F; // Key to interact
    private bool isPlayerNearby = false;

    public CinemachineVirtualCamera mainCamera; // Assign in Inspector
    public CinemachineVirtualCamera puzzleCamera; // Assign in Inspector
    public GameObject puzzleManager; // Reference to PuzzleManager

    void Start()
    {
        // Ensure normal game starts with main camera active
        if (mainCamera != null)
            mainCamera.Priority = 11; // Highest priority for start

        if (puzzleCamera != null)
            puzzleCamera.Priority = 0; // Lower priority so it's inactive

        if (puzzleManager != null)
            puzzleManager.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(interactionKey))
        {
            EnterPuzzleMode();
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) // Press ESC to exit puzzle mode
        {
            ExitPuzzleMode();
        }
    }

    void EnterPuzzleMode()
    {
        Debug.Log("Switching to Puzzle Mode...");
        if (puzzleCamera != null && mainCamera != null)
        {
            puzzleCamera.Priority = 12; // Make puzzle cam active
            mainCamera.Priority = 0; // Deactivate main cam
        }

        if (puzzleManager != null)
            puzzleManager.SetActive(true);
    }

    void ExitPuzzleMode()
    {
        Debug.Log("Exiting Puzzle Mode...");
        if (puzzleCamera != null && mainCamera != null)
        {
            puzzleCamera.Priority = 0; // Deactivate puzzle cam
            mainCamera.Priority = 12; // Reactivate main cam
        }

        if (puzzleManager != null)
            puzzleManager.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
    public Camera GetActiveCamera()
    {
        if (puzzleCamera.Priority > mainCamera.Priority)
        {
            return puzzleCamera.GetComponent<Camera>(); // Ensure the puzzle cam has a Camera component
        }
        return mainCamera.GetComponent<Camera>(); // Ensure the main cam has a Camera component
    }
}
