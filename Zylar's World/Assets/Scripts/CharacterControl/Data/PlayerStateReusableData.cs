using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateReusableData
{
    public Vector2 movementInput { get; set; }
    public float movementSpeedMultiplier { get; set; } = 1f;
    public float movementOnSlopeSpeedMultiplier { get; set; } = 1f;
    public float movementDecelerationForce { get; set; } = 1f;

    public List<PlayerCameraRecenteringData> sidewaysCameraRecenteringData { get; set; }
    public List<PlayerCameraRecenteringData> backwardsCameraRecenteringData { get; set; }

    public bool shouldWalk { get; set; }
    public bool shouldSprint { get; set; }
    private Vector3 currentTargetRotation;
    private Vector3 timeToReachTargetRotation;
    private Vector3 dampedTargetRotationCurrentVelocity;
    private Vector3 dampedTargetRotationPassedTime;
    public ref Vector3 CurrentTargetRotation
    {
        get
        {
            return ref currentTargetRotation;
        }
    }
    public ref Vector3 TimeToReachTargetRotation
    {
        get
        {
            return ref timeToReachTargetRotation;
        }
    }
    public ref Vector3 DampedTargetRotationCurrentVelocity
    {
        get
        {
            return ref dampedTargetRotationCurrentVelocity;
        }
    }
    public ref Vector3 DampedTargetRotationPassedTime
    {
        get
        {
            return ref dampedTargetRotationPassedTime;
        }
    }
    public Vector3 currentJumpForce { get; set; }
    public PlayerRotationData rotationData { get; set; }
}
