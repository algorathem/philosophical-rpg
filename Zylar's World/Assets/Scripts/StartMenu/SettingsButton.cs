using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsButton : MonoBehaviour, IPointerClickHandler
{
    private MainMenuController mainMenuController;  // Reference to the MainMenuController
    // Start is called before the first frame update
    void Start()
    {
        mainMenuController = FindObjectOfType<MainMenuController>();  // Find the MainMenuController
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Call the OnSettingsButtonClicked method from the MainMenuController
        if (mainMenuController != null && mainMenuController.IsMenuInteractable)
        {
            mainMenuController.OnSettingsButtonClicked();
        }
    }

}
