using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTestingPlayerMove : MonoBehaviour
{
    private bool hasTriggeredDialogue = false; // Prevent multiple triggers

    void Update()
    {
        // Detect movement using WASD or arrow keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        // Log input values
        Debug.Log($"Horizontal: {moveHorizontal}, Vertical: {moveVertical}");

        if (!hasTriggeredDialogue && (moveHorizontal != 0 || moveVertical != 0))
        {
            hasTriggeredDialogue = true;
            TriggerDialogue();
        }

        // Check if player moves and trigger dialogue
        if (!hasTriggeredDialogue && (moveHorizontal != 0 || moveVertical != 0))
        {
            hasTriggeredDialogue = true; // Ensure it only triggers once
            TriggerDialogue();
        }
    }

    private void TriggerDialogue()
    {
        if (PersistentDialogueManager.Instance != null)
        {
            Debug.Log("PersistentDialogueManager found, starting dialogue...");
            PersistentDialogueManager.Instance.dialogueRunner.StartDialogue("Start");
        }
        else
        {
            Debug.LogError("PersistentDialogueManager.Instance is null!");
        }

    }
}
