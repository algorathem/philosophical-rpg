using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerRunData
{
    [field: SerializeField][field: Range(1f, 2f)] public float speedMultiplier { get; private set; } = 1f;
}
