using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DefaultColliderData
{
    [field: SerializeField] public float height { get; private set; } = 1.8f;
    [field: SerializeField] public float centerY { get; private set; } = 0.9f;
    [field: SerializeField] public float radius { get; private set; } = 0.2f;

}
