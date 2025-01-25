using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerStopData
{
    [field: SerializeField][field: Range(0f, 15f)] public float lightDecelerationForce { get; private set; } = 5f;
    [field: SerializeField][field: Range(0f, 15f)] public float mediumDecelerationForce { get; private set; } = 6.5f;
    [field: SerializeField][field: Range(0f, 15f)] public float hardDecelerationForce { get; private set; } = 5f;
}
