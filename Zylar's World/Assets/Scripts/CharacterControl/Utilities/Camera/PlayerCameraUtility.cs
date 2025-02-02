using UnityEngine;
using System;
using Cinemachine;

[Serializable]
public class PlayerCameraUtility
{
    [field: Header("Camera Target Settings")]
    [field: SerializeField] public Transform cameraTarget { get; private set; }  // Reference to camera target
    [field: SerializeField] public float aimOffset { get; private set; } = 1f;
    [field: SerializeField] public float targetReachThreshold { get; private set; } = 0.01f;

    [field: Header("Camera Settings")]
    [field: SerializeField] public CinemachineVirtualCamera virtualCamera { get; private set; }
    [field: SerializeField] public float defaultHorizontalWaitTime { get; private set; } = 0f;
    [field: SerializeField] public float defaultHorizontalRecenteringTime { get; private set; } = 4f;

    private CinemachinePOV cinemachinePOV;
    public Vector3 originalTargetPosition { get; private set; }  // The original position of the camera target
    public Vector3 finalTargetPosition { get; private set; }  // The final position of the camera target

    public void Initialize()
    {
        cinemachinePOV = virtualCamera.GetCinemachineComponent<CinemachinePOV>();

        originalTargetPosition = cameraTarget.localPosition;
        finalTargetPosition = originalTargetPosition + Vector3.up * aimOffset;
    }

    public void EnableRecentering(float waitTime = -1f, float recenteringTime = -1f, float baseMovementSpeed = 1f, float movementSpeed = 1f)
    {
        cinemachinePOV.m_HorizontalRecentering.m_enabled = true;

        cinemachinePOV.m_HorizontalRecentering.CancelRecentering();

        if (waitTime == -1f)
        {
            waitTime = defaultHorizontalWaitTime;
        }

        if (recenteringTime == -1f)
        {
            recenteringTime = defaultHorizontalRecenteringTime;
        }

        recenteringTime = recenteringTime * baseMovementSpeed / movementSpeed;

        cinemachinePOV.m_HorizontalRecentering.m_WaitTime = waitTime;
        cinemachinePOV.m_HorizontalRecentering.m_RecenteringTime = recenteringTime;
    }

    public void DisableRecentering()
    {
        cinemachinePOV.m_HorizontalRecentering.m_enabled = false;
    }
}
