using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerCapsuleColliderUtility : CapsuleColliderUtility
{
    [field: SerializeField] public PlayerTriggerColliderData triggercolliderData { get; private set; }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        triggercolliderData.Initialize();
    }
}
