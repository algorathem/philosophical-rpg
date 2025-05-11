// using UnityEngine;
// using Yarn.Unity;

// public class YarnUserActionDetector : MonoBehaviour
// {
//     public string yarnBoolCondition = "accessCard"; // Yarn variable
//     public string yarnNodeToStart = "UpdatedTutorial";

//     private DialogueRunner dialogueRunner;
//     private InMemoryVariableStorage yarnMemory;

//     void Start()
//     {
//         dialogueRunner = FindObjectOfType<DialogueRunner>();
//         yarnMemory = dialogueRunner.GetComponent<InMemoryVariableStorage>();
//     }

//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.F) && !dialogueRunner.IsDialogueRunning)
//         {
//             if (yarnMemory != null && yarnMemory.GetValue("$" + yarnBoolCondition).AsBool)
//             {
//                 dialogueRunner.StartDialogue(yarnNodeToStart);
//             }
//         }
//     }
// }
