using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBox : MonoBehaviour
{
    public int index = 0;
    int x = 0;
    int y = 0;

    private float spriteWidth;
    private float spriteHeight;
    private SpriteRenderer spriteRenderer;

    private Action<int, int> swapFunc = null;

    public void Init(int i, int j,Vector2 offset, int index, Sprite sprite, Action<int, int> swapFunc)
    {
        this.index = index;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        // Get sprite dimensions
        spriteWidth = sprite.bounds.size.x;
        spriteHeight = sprite.bounds.size.y;

        // Highlight empty space (index 15)
        if (IsEmpty())
        {
            HighlightEmptyPiece();
        }

        UpdatePos(i, j, spriteWidth, spriteHeight,offset);
        this.swapFunc = swapFunc;
    }

    public void UpdatePos(int i, int j, float width, float height,Vector2 offset)
    {
        x = i;
        y = j;
        StartCoroutine(Move(width, height,offset));
    }

    IEnumerator Move(float width, float height, Vector2 offset)
    {
        float elapsedTime = 0;
        float duration = 0.1f; // Animation Speed
        Vector2 start = this.gameObject.transform.localPosition;
        Vector2 end = new Vector2(x * width, y * height) + offset;

        while (elapsedTime < duration)
        {
            this.gameObject.transform.localPosition = Vector2.Lerp(start, end, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.gameObject.transform.localPosition = end;
    }

    public bool IsEmpty()
    {
        return index == 16; // Element 15 is the blank space
    }

    private void HighlightEmptyPiece()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f); // Semi-transparent white
        }
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && swapFunc != null)
        {
            swapFunc(x, y);
        }
    }
}
