using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerFallData
{
    [field: Tooltip("Having higher numbers might not read collisions with shallow colliders correctly.")]
    [field: SerializeField][field: Range(0f, 10f)] public float fallSpeedLimit { get; private set; } = 10f;
    [field: SerializeField][field: Range(0f, 100f)] public float minimumDistanceToBeConsideredHardFall { get; private set; } = 3f;
}
