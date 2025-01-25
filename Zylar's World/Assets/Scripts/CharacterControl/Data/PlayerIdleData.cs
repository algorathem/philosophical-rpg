using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerIdleData
{
    [field: SerializeField] public List<PlayerCameraRecenteringData> backwardsCameraRecenteringData { get; private set; }
}
