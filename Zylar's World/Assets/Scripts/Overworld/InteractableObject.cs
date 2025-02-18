using Cinemachine;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public KeyCode interactionKey = KeyCode.F; // Key to interact
    private bool isPlayerNearby = false;

    public Texture2D cursorForCam1;

    public CinemachineBrain cinemachineBrain;
    public CinemachineVirtualCamera mainCamera; // Assign in Inspector
    public CinemachineVirtualCamera puzzleCamera; // Assign in Inspector
    public GameObject puzzleManager; // Reference to PuzzleManager
    public CursorTrail trailForCam1;

    void Start()
    {
        // Ensure normal game starts with main camera active
        if (mainCamera != null)
            mainCamera.Priority = 11; // Highest priority for start

        if (puzzleCamera != null)
            puzzleCamera.Priority = 0; // Lower priority so it's inactive

        if (puzzleManager != null)
            puzzleManager.SetActive(false);

        if (cinemachineBrain == null)
            cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();

        // Listen for camera switch events
        cinemachineBrain.m_CameraActivatedEvent.AddListener(OnCameraSwitch);
        trailForCam1.enabled = false;
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

    private void OnCameraSwitch(ICinemachineCamera newCam, ICinemachineCamera oldCam)
    {
        if (newCam != null)
        {
            Debug.Log("Switched to Camera: " + newCam.VirtualCameraGameObject.name);

            // Disable cursor and trail by default
            Cursor.visible = false;
            trailForCam1.enabled = false;

            if (newCam.VirtualCameraGameObject.name == "Connect Cam")
            {
                Cursor.visible = true;
                Cursor.SetCursor(cursorForCam1, Vector2.zero, CursorMode.Auto);
                trailForCam1.enabled = true;
                Debug.Log("Cursor & Trail Enabled");
            }
            else
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                Debug.Log("Cursor & Trail Disabled");
            }
        }
    }


}
