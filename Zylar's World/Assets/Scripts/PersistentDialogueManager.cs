using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PersistentDialogueManager : MonoBehaviour
{
    public static PersistentDialogueManager Instance;
    public DialogueRunner dialogueRunner;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make it persistent
        }
        else
        {
            Destroy(gameObject); // Avoid duplicates
        }
    }
}
