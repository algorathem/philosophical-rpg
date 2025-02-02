using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public NumberBox boxPrefab;
    public NumberBox[,] boxes = new NumberBox[4, 4];
    public Sprite[] sprites;
    public int shuffleCount;

    private float spriteWidth;
    private float spriteHeight;
    public Vector2 offset = new Vector2(0,30f);

    void Start()
    {
        if (sprites.Length > 0)
        {
            spriteWidth = sprites[0].bounds.size.x;  // Assuming all sprites have the same width
            spriteHeight = sprites[0].bounds.size.y; // Assuming all sprites have the same height
        }
        Init();
        for (int i = 0; i < shuffleCount; i++)
        {
            Shuffle();
        }
    }

    void Init()
    {
        int n = 0;
        for (int y = 3; y >= 0; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                Vector2 position = new Vector2(x * spriteWidth, y * spriteHeight) + offset;
                NumberBox box = Instantiate(boxPrefab, position, Quaternion.identity);
                box.Init(x, y, offset, n + 1, sprites[n], ClickToSwap);
                boxes[x, y] = box;
                n++;
            }
        }
    }

    void ClickToSwap(int x, int y)
    {
        int dx = GetDx(x, y);
        int dy = GetDy(x, y);
        Swap(x, y, dx, dy);
    }

    void Swap(int x, int y, int dx, int dy)
    {
        var prevbox = boxes[x, y];
        var targetbox = boxes[x + dx, y + dy];

        boxes[x, y] = targetbox;
        boxes[x + dx, y + dy] = prevbox;

        prevbox.UpdatePos(x + dx, y + dy, spriteWidth, spriteHeight,offset);
        targetbox.UpdatePos(x, y, spriteWidth, spriteHeight,offset);
    }

    int GetDx(int x, int y)
    {
        if (x < 3 && boxes[x + 1, y].IsEmpty()) return 1;
        if (x > 0 && boxes[x - 1, y].IsEmpty()) return -1;
        return 0;
    }

    int GetDy(int x, int y)
    {
        if (y < 3 && boxes[x, y + 1].IsEmpty()) return 1;
        if (y > 0 && boxes[x, y - 1].IsEmpty()) return -1;
        return 0;
    }

    void Shuffle()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (boxes[i, j].IsEmpty())
                {
                    Vector2 pos = getValidMove(i, j);
                    Swap(i, j, (int)pos.x, (int)pos.y);
                }
            }
        }
    }

    private Vector2 lastMove;

    Vector2 getValidMove(int x, int y)
    {
        Vector2 pos = new Vector2();
        do
        {
            int n = UnityEngine.Random.Range(0, 4);
            switch (n)
            {
                case 0: pos = Vector2.left; break;
                case 1: pos = Vector2.right; break;
                case 2: pos = Vector2.up; break;
                default: pos = Vector2.down; break;
            }
        } while (!(IsValidRange(x + (int)pos.x) && IsValidRange(y + (int)pos.y)) || IsRepeatMove(pos));

        lastMove = pos;
        return pos;
    }

    bool IsValidRange(int n)
    {
        return n >= 0 && n <= 3;
    }

    bool IsRepeatMove(Vector2 pos)
    {
        return pos * -1 == lastMove;
    }
}
