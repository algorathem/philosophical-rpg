using UnityEngine;
using System;

[Serializable]
public class PlayerTriggerColliderData
{
    [field: SerializeField] public BoxCollider groundCheckCollider { get; private set; }

    public Vector3 groundCheckColliderExtents { get; private set; }

    public void Initialize()
    {
        groundCheckColliderExtents = groundCheckCollider.bounds.extents;
    }
}
