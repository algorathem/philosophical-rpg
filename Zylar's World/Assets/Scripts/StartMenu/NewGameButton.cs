using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour, IPointerClickHandler
{
    private MainMenuController mainMenuController;  // Reference to the MainMenuController

    // Start is called before the first frame update
    void Start()
    {
        mainMenuController = FindObjectOfType<MainMenuController>();  // Find the MainMenuController
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (SceneController.instance != null)
        {
            SceneController.instance.LoadSceneByName("Start");  // Calls the coroutine with fade
        }
        else
        {
            Debug.LogWarning("SceneController not found!");
        }
    }
}

