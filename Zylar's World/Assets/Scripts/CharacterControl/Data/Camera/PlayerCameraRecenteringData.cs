using UnityEngine;
using System;

[Serializable]
public class PlayerCameraRecenteringData
{
    [field: SerializeField][field: Range(0f, 360f)] public float minimumAngle { get; private set; }
    [field: SerializeField][field: Range(0f, 360f)] public float maximumAngle { get; private set; }
    [field: SerializeField][field: Range(-1f, 20f)] public float waitTime { get; private set; }
    [field: SerializeField][field: Range(-1f, 20f)] public float recenteringTime { get; private set; }

    public bool IsWithinRange(float angle)
    {
        return angle >= minimumAngle && angle <= maximumAngle;
    }
}
