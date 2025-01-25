using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerGroundedData
{
    [field: SerializeField][field: Range(0f, 25f)] public float baseSpeed { get; private set; } = 5f;
    [field: SerializeField][field: Range(0f, 5f)] public float groundToFallRayDistance { get; private set; } = 1f;
    [field: SerializeField] public AnimationCurve slopeSpeedAngle { get; private set; }
    [field: SerializeField] public List<PlayerCameraRecenteringData> sidewaysCameraRecenteringData { get; private set; }
    [field: SerializeField] public List<PlayerCameraRecenteringData> backwardsCameraRecenteringData { get; private set; }
    [field: SerializeField] public PlayerRotationData baseRotationData { get; private set; }
    [field: SerializeField] public PlayerWalkData walkData { get; private set; }
    [field: SerializeField] public PlayerIdleData idleData { get; private set; }
    [field: SerializeField] public PlayerRunData runData { get; private set; }
    [field: SerializeField] public PlayerDashData dashData { get; private set; }
    [field: SerializeField] public PlayerSprintData sprintData { get; private set; }
    [field: SerializeField] public PlayerStopData stopData { get; private set; }
    [field: SerializeField] public PlayerRollData rollData { get; private set; }
}
