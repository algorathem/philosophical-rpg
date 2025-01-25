using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerDashData
{
    [field: SerializeField][field: Range(1f, 3f)] public float speedMultiplier { get; private set; } = 2f;
    [field: SerializeField] public PlayerRotationData rotationData { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float dashCooldown { get; private set; } = 0.5f;
}
