using UnityEngine;
using Yarn.Unity;
using TMPro;

public class ImmortalTrigger : MonoBehaviour
{
    public string dialogueNode = "ImmortalRevelation"; 
    private DialogueRunner dialogueRunner;
    public TextMeshProUGUI continuePrompt;

    private bool playerInRange = false;

    private void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();

        if (dialogueRunner == null)
        {
            Debug.LogError("DialogueRunner not found in the scene.");
        }

        if (continuePrompt != null)
        {
            continuePrompt.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (continuePrompt != null)
                continuePrompt.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (continuePrompt != null)
                continuePrompt.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F) && dialogueRunner != null && !dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.StartDialogue(dialogueNode);
            if (continuePrompt != null)
                continuePrompt.gameObject.SetActive(false);
        }
    }
}
