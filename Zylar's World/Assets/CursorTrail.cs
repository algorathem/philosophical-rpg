using UnityEngine;

public class CursorTrail : MonoBehaviour
{
    public Camera uiCamera; // Assign your UI or main camera in Inspector
    private TrailRenderer trailRenderer;

    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = uiCamera.nearClipPlane + 0.1f; // Adjust Z depth

        Vector3 worldPos = uiCamera.ScreenToWorldPoint(mousePosition);
        transform.position = worldPos;
    }
}
