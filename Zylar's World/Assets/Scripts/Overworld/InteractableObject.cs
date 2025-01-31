using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableObject : MonoBehaviour
{
    //public string targetScene; // Name of the scene to load
    public KeyCode interactionKey = KeyCode.F; // Key to interact

    private bool isPlayerNearby = false;
    public CinemachineVirtualCamera mainCamera; // Assign in the inspector
    public CinemachineVirtualCamera puzzleCamera; // Assign in the inspector
    public GameObject puzzleManager; // Reference to the PuzzleManager

    void Start()
    {
        /*
        // Ensure the main camera is active at the start
        if (mainCamera != null)
            mainCamera.Priority = 10;

        if (puzzleCamera != null)
            puzzleCamera.Priority = 0;*/
        puzzleManager.SetActive(false);
    }
    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(interactionKey))
        {
            //SceneManager.LoadScene(targetScene);
            EnterPuzzleMode();
        }
    }
    void EnterPuzzleMode()
    {
        Debug.Log("Switching to Puzzle Mode...");

        // Switch camera priority to transition to the puzzle view
        if (puzzleCamera != null && mainCamera != null)
        {
            puzzleCamera.Priority = 11; // Higher priority to activate
            mainCamera.Priority = 0; // Lower priority to deactivate
        }

        // Enable the puzzle interaction
        if (puzzleManager != null)
        {
            puzzleManager.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the "Player" tag
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
}
