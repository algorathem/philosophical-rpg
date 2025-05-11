using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;

public class DialogueInteractionTrigger : MonoBehaviour
{
    public string yarnNode = "Machiavelli";
    public GameObject promptUI; // UI element saying "Press F to interact"
    private DialogueRunner dialogueRunner;
    private bool playerInRange = false;

    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F) && !dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.StartDialogue(yarnNode);
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }
}
