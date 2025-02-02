using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SlopeData
{
    [field: SerializeField][field: Range(0f, 1f)] public float stepHeightPercentage { get; private set; } = 0.25f;
    [field: SerializeField][field: Range(0f, 5f)] public float floatRayDistance { get; private set; } = 2f;
    [field: SerializeField][field: Range(0f, 50f)] public float stepReachForce { get; private set; } = 25f;
}
