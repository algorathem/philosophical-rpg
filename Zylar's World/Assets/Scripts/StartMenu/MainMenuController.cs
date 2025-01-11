using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenuController : MonoBehaviour
{
    public float fadeTime = 0.6f;  // Time it takes for the menu to fade in
    public float logoFadeTime = 0.8f; // Time it takes for the logo to fade in
    public float delayTime = 0.3f;  // Delay before the menu fades in
    public CanvasGroup mainMenuGroup;  // Reference to the main menu's CanvasGroup component
    public CanvasGroup logoGroup;  // Reference to the logo's CanvasGroup component
    public CanvasGroup menuGroup;  // Reference to the menu's CanvasGroup component
    public CanvasGroup settingsGroup;  // Reference to the settings menu's CanvasGroup component

    public bool IsMenuInteractable;  // Flag to check if the start menu is interactable
    public bool IsSettingsInteractable;  // Flag to check if the settings menu is interactable


    // Start is called before the first frame update
    void Start()
    {
        // Set the interactability of the menu to false
        IsMenuInteractable = false;
        IsSettingsInteractable = false;

        // Start the fade in sequence
        StartFadeIn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartFadeIn()
    {
        // Create a sequence to fade in the logo and menu
        Sequence fadeInSequence = DOTween.Sequence();

        // Fade in the logo
        fadeInSequence.Append(logoGroup.DOFade(0f, logoFadeTime).From().SetEase(Ease.InOutQuad));

        // Add a delay before fading in the menu
        fadeInSequence.AppendInterval(delayTime);

        // Fade in the menu
        fadeInSequence.Append(menuGroup.DOFade(0f, fadeTime).From().SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            // Set the interactability of the menu to true
            IsMenuInteractable = true;
        }));

        // Play the fade in sequence
        fadeInSequence.Play();
    }

    public void OnSettingsButtonClicked()
    {
        // Check if the settings menu is interactable
        if (IsMenuInteractable)
        {
            // Set the interactability of the menu to false
            IsMenuInteractable = false;

            // Start sequence to fade out the menu and fade in the settings menu
            Sequence startToSettingsSequence = DOTween.Sequence();

            // Fade out the menu
            startToSettingsSequence.Append(mainMenuGroup.DOFade(0f, fadeTime).SetEase(Ease.InOutQuad));

            // Fade in the settings menu
            startToSettingsSequence.Append(settingsGroup.DOFade(1f, fadeTime).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                // Set the interactability of the settings menu to true
                IsSettingsInteractable = true;
            }));

            // Play the sequence
            startToSettingsSequence.Play();
        }
    }



}
