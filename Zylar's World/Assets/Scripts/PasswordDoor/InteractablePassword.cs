using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //To load a new scene when password is correct

public class InteractablePassword : MonoBehaviour
{
    public GameObject passwordUIPanel; // Assign the UI panel in the Inspector
    public TMP_InputField passwordInputField; // TMP Input Field
    public TextMeshProUGUI feedbackText; // TMP Text for feedback
    public Button submitButton; // Assign a UI Button in Inspector
    public string correctPassword = "1234"; // Set your password
    public string sceneName; //Set scene to load on completion of puzzle
    private bool isPlayerNearby = false;

    void Start()
    {
        passwordUIPanel.SetActive(false); // Hide UI initially
        passwordInputField.onSubmit.AddListener(delegate { CheckPassword(); }); // Detect "Enter" key
        submitButton.onClick.AddListener(CheckPassword); // Button click triggers password check
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F)) // Press 'F' to interact
        {
            passwordUIPanel.SetActive(true);
            passwordInputField.text = ""; // Clear previous input
            passwordInputField.ActivateInputField();
        }
    }

    public void CheckPassword()
    {
        if (passwordInputField.text == correctPassword)
        {
            feedbackText.text = "Access Granted!";
            passwordUIPanel.SetActive(false);
            PerformAction();
        }
        else
        {
            feedbackText.text = "Access denied. Re-enter sequence.";
            passwordInputField.text = ""; // Clear wrong input
            passwordInputField.ActivateInputField();
        }
    }

    void PerformAction()
    {
        Debug.Log("Correct password entered! Performing action...");
        // Implement an action here (e.g., open a door, reveal an object)
        if (SceneController.instance != null)
        {
            SceneController.instance.LoadSceneByName(sceneName);
        }
        else
        {
            Debug.LogWarning("SceneController not found, loading directly.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            passwordUIPanel.SetActive(false); // Hide UI if player leaves
        }
    }
}
