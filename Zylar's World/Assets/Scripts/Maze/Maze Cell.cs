using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] private GameObject unvisited;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject frontWall;
    [SerializeField] private GameObject backWall;

    public bool IsVisited { get; private set; }  // Tracks if the cell is visited
    public float GCost { get; set; }  // Cost from the start node to this cell
    public float HCost { get; set; }  // Estimated cost from this cell to the goal
    public MazeCell Parent { get; set; }  // The parent cell for path reconstruction
    public float FCost { get { return GCost + HCost; } }  // FCost = GCost + HCost

    // Property to check if the current cell is a wall
    public bool IsWall
    {
        get
        {
            // Consider the cell a wall if any of the walls (left, right, front, back) are active
            return leftWall.activeSelf || rightWall.activeSelf || frontWall.activeSelf || backWall.activeSelf;
        }
    }

    public void Visit()
    {
        IsVisited = true;
        unvisited.SetActive(false);  // Hide unvisited indicator
    }

    public void ClearLeftWall()
    {
        leftWall.SetActive(false);  // Disable left wall
    }

    public void ClearRightWall()
    {
        rightWall.SetActive(false);  // Disable right wall
    }

    public void ClearFrontWall()
    {
        frontWall.SetActive(false);  // Disable front wall
    }

    public void ClearBackWall()
    {
        backWall.SetActive(false);  // Disable back wall
    }
}
