using UnityEngine;
using System;

[Serializable]
public class PlayerRollData
{
    [field: SerializeField][field: Range(0f, 3f)] public float speedMultiplier { get; private set; } = 1f;
}
