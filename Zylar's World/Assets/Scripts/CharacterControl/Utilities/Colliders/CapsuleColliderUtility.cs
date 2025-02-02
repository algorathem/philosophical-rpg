using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CapsuleColliderUtility
{
  public CapsuleColliderData capsuleColliderData { get; private set; }
  [field: SerializeField] public DefaultColliderData defaultColliderData { get; private set; }
  [field: SerializeField] public SlopeData slopeData { get; private set; }
  public void Initialize(GameObject gameObject)
  {
    if (capsuleColliderData != null)
    {
      return;
    }

    capsuleColliderData = new CapsuleColliderData();
    capsuleColliderData.Initialize(gameObject);

    OnInitialize();
  }

  protected virtual void OnInitialize()
  {

  }

  public void CalculateCapsuleColliderDimension()
  {
    SetCapsuleColliderRadius(defaultColliderData.radius);
    SetCapsuleColliderHeight(defaultColliderData.height * (1f - slopeData.stepHeightPercentage));
    RecalculateCapsuleColliderCenter();

    float halfColliderHeight = capsuleColliderData.collider.height / 2f;
    if (halfColliderHeight < capsuleColliderData.collider.radius)
    {
      SetCapsuleColliderRadius(halfColliderHeight);
    }

    capsuleColliderData.UpdateColliderData();
  }

  public void SetCapsuleColliderRadius(float radius)
  {
    capsuleColliderData.collider.radius = radius;
  }

  public void SetCapsuleColliderHeight(float height)
  {
    capsuleColliderData.collider.height = height;
  }

  public void RecalculateCapsuleColliderCenter()
  {
    float colliderHeightDifference = defaultColliderData.height - capsuleColliderData.collider.height;
    Vector3 newColliderCenter = new Vector3(0f, defaultColliderData.centerY + (colliderHeightDifference / 2f), 0f);
    capsuleColliderData.collider.center = newColliderCenter;
  }

}
