using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public NumberBox boxPrefab;
    public NumberBox[,] boxes = new NumberBox[4, 4];
    public Sprite[] sprites;
    public int shuffleCount;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        for (int i =0;i < shuffleCount; i++)
        {
            Shuffle();
        }
        
    }
    void Init()
    {
        int n = 0;
        for(int y = 3; y >= 0; y--)
        {
            for(int x=0; x < 4; x++)
            {
                NumberBox box = Instantiate(boxPrefab,new Vector2(x,y),Quaternion.identity);
                box.Init(x, y, n + 1, sprites[n], ClickToSwap);
                boxes[x,y] = box;
                n++;
            }
        }    
    }

    void ClickToSwap(int x,int y)
    {
        int dx = GetDx(x, y);
        int dy = GetDy(x, y);
        Swap(x, y, dx, dy);
    }
    void Swap(int x,int y, int dx, int dy)
    {
        var prevbox = boxes[x, y];
        var targetbox = boxes[x+dx, y+dy];
        boxes[x, y] = targetbox;
        boxes[x+dx, y+dy] = prevbox;

        prevbox.UpdatePos(x+dx, y+dy);
        targetbox.UpdatePos(x, y);
    }

    int GetDx(int x,int y)
    {
        //is right empty
        if(x<3 && boxes[x + 1, y].IsEmpty())
        {
            return 1;
        }
        //is left empty
        if(x > 0 && boxes[x - 1, y].IsEmpty())
        {
            return -1;
        }
        return 0;
    }
    int GetDy(int x,int y)
    {
        //is top empty
        if (y < 3 && boxes[x, y+1].IsEmpty())
        {
            return 1;
        }
        //is bottom empty
        if (y > 0 && boxes[x, y-1].IsEmpty())
        {
            return -1;
        }
        return 0;
    }

    void Shuffle()
    {
        for(int i=0; i<4; i++)
        {
            for(int j=0;j<4; j++)
            {
                if (boxes[i,j].IsEmpty())
                {
                    Vector2 pos = getValidMove(i, j);
                    Swap(i, j,(int)pos.x,(int)pos.y);
                }
            }
        }
    }

    private Vector2 lastMove; 

    Vector2 getValidMove(int x,int y)
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
        return pos*-1 == lastMove;
    }
}
