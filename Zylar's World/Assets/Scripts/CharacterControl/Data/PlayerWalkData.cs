using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerWalkData
{
    [field: SerializeField][field: Range(0f, 1f)] public float speedMultiplier { get; private set; } = 0.225f;
    [field: SerializeField] public List<PlayerCameraRecenteringData> backwardsCameraRecenteringData { get; private set; }
}
