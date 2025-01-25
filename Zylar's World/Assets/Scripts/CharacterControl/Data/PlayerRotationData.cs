using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerRotationData
{
    [field: SerializeField] public Vector3 targetRotationReachTime { get; private set; }
}
