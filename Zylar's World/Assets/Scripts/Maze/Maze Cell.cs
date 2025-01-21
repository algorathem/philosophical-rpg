using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
   [SerializeField] private GameObject leftWall;
   [SerializeField] private GameObject rightWall;
   [SerializeField] private GameObject frontWall;
   [SerializeField] private GameObject backWall;
   [SerializeField] private GameObject unvisited;
    public bool IsVisited { get; private set; } 
    public void Visit()
    {
        IsVisited = true;
        unvisited.SetActive(false);
    }

    public void ClearLeftWall()
    {
        leftWall.SetActive(false);
    }

    public void ClearRightWall()
    {
        rightWall.SetActive(false);
    }

    public void ClearFrontWall()
    {
        frontWall.SetActive(false);
    }

    public void ClearBackWall()
    {
        backWall.SetActive(false);
    }
}

