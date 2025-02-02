using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsAudioButton : MonoBehaviour, IPointerClickHandler
{
    private MainMenuController mainMenuController;  // Reference to the MainMenuController

    // Start is called before the first frame update
    void Start()
    {
        mainMenuController = FindObjectOfType<MainMenuController>();  // Find the MainMenuController
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        mainMenuController.OnSettingsAudioButtonClicked();
    }
}

