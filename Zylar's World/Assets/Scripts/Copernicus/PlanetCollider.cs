using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlanetCollider : MonoBehaviour
{
    private bool isHovered = false;
    private bool isInteracted = false;
    public float outlineWidth = 5.0f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        OutlineOnHover();

    }

    private void OnMouseEnter()
    {
        isHovered = true;
        // transform.DOLocalMoveZ(originalPosition.z + 2f, 1f).SetEase(Ease.OutQuad);
    }

    private void OnMouseExit()
    {
        isHovered = false;
        // transform.DOLocalMoveZ(originalPosition.z, 1f).SetEase(Ease.OutQuad);
    }


    private void OutlineOnHover()
    {
        if (isHovered)
        {
            if (GetComponent<Outline>() != null)
            {
                GetComponent<Outline>().enabled = true;
            }
            else
            {
                Outline outline = gameObject.AddComponent<Outline>();
                outline.enabled = true;
                outline.OutlineColor = Color.white;
                outline.OutlineWidth = outlineWidth;
            }
        }
        else
        {
            if (GetComponent<Outline>() != null)
            {
                GetComponent<Outline>().enabled = false;
            }
        }
    }


}
