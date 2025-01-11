using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scaleFactor = 1.1f;  // Scale factor for the button when hovered over
    public float scaleTime = 0.2f;  // Time it takes for the button to scale up
    public float hoverVolume = 1f;  // Volume of the hover sound effect
    [SerializeField] private AudioClip hoverClip;  // Sound effect to play when the button is hovered over

    private Vector3 originalScale;  // Original scale of the button


    // Start is called before the first frame update
    void Start()
    {


        originalScale = transform.localScale;  // Store the original scale of the button
    }


    public void OnPointerEnter(PointerEventData eventData)
    {


        // Play the hover sound effect
        SFXManager.instance.PlaySFXClip(hoverClip, transform, hoverVolume);


        // Scale up the button
        transform.DOScale(originalScale * scaleFactor, scaleTime).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        // Scale down the button
        transform.DOScale(originalScale, scaleTime).SetEase(Ease.OutBack);
    }
}
