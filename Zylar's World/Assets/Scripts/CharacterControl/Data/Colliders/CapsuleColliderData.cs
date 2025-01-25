using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleColliderData
{
    public CapsuleCollider collider { get; private set; }
    public Vector3 colliderCenterInLocalSpace { get; private set; }
    public Vector3 colliderVerticalExtents { get; private set; }

    public void Initialize(GameObject gameObject)
    {
        if (collider != null)
        {
            return;
        }

        collider = gameObject.GetComponent<CapsuleCollider>();
        UpdateColliderData();
    }

    public void UpdateColliderData()
    {
        colliderCenterInLocalSpace = collider.center;
        colliderVerticalExtents = new Vector3(0f, collider.bounds.extents.y, 0f);
    }
}
