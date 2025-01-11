using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenuController : MonoBehaviour
{
    public float fadeTime = 0.6f;  // Time it takes for the menu to fade in
    public float logoFadeTime = 0.8f; // Time it takes for the logo to fade in
    public float delayTime = 0.3f;  // Delay before the menu fades in
    public CanvasGroup logoGroup;  // Reference to the logo's CanvasGroup component
    public CanvasGroup menuGroup;  // Reference to the menu's CanvasGroup component


    // Start is called before the first frame update
    void Start()
    {
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
        fadeInSequence.Append(menuGroup.DOFade(0f, fadeTime).From().SetEase(Ease.InOutQuad));

        // Play the fade in sequence
        fadeInSequence.Play();
    }


}
