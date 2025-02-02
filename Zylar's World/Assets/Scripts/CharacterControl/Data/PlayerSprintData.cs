using UnityEngine;
using System;

[Serializable]
public class PlayerSprintData
{
    [field: SerializeField][field: Range(1f, 3f)] public float speedMultiplier { get; private set; } = 1.7f;
    [field: SerializeField][field: Range(0f, 5f)] public float sprintToRunTime { get; private set; } = 1f;
    [field: SerializeField][field: Range(0f, 2f)] public float runToWalkTime { get; private set; } = 0.5f;

}
