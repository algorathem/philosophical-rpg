using UnityEngine;
using DG.Tweening;

public class PlanetCollider : MonoBehaviour
{
    public float outlineWidth = 5.0f;

    [Header("Orbit Radius Snapping")]
    public Transform orbitCenter;       // Usually the Sun
    public float targetOrbitRadius = 10f;
    public float snapRadiusTolerance = 1f;
    public float snapDuration = 0.3f;
    public bool isSun = false;

    private bool isHovered = false;
    private bool isDragging = false;
    private Vector3 originalPosition;
    private Vector3 dragOffset;

    public bool isPlacedCorrectly = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        OutlineOnHover();
    }

    private void OnMouseEnter()
    {
        isHovered = true;
    }

    private void OnMouseExit()
    {
        isHovered = false;
    }

    private void OnMouseDown()
    {
        if (isHovered)
        {
            isDragging = true;

            Plane dragPlane = new Plane(Vector3.up, orbitCenter.position); // drag relative to orbit plane
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                dragOffset = transform.position - hitPoint;
            }
        }
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Plane dragPlane = new Plane(Vector3.up, orbitCenter.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                transform.position = hitPoint + dragOffset;
            }
        }
    }

    private void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;

            float distanceToOrbit = Vector3.Distance(transform.position, orbitCenter.position);
            if (Mathf.Abs(distanceToOrbit - targetOrbitRadius) <= snapRadiusTolerance)
            {
                // Snap to correct orbit radius, keep angle the same
                Vector3 direction = (transform.position - orbitCenter.position).normalized;
                Vector3 snappedPosition = orbitCenter.position + direction * targetOrbitRadius;

                transform.DOMove(snappedPosition, snapDuration).SetEase(Ease.OutBack);
                isPlacedCorrectly = true;
                PuzzleManager.Instance.CheckCompletion();
            }
            else
            {
                transform.DOMove(originalPosition, snapDuration).SetEase(Ease.OutQuad);
                isPlacedCorrectly = false;
            }
        }
    }

    private void OutlineOnHover()
    {
        Outline outline = GetComponent<Outline>();
        if (isHovered)
        {
            if (outline == null)
            {
                outline = gameObject.AddComponent<Outline>();
                outline.OutlineColor = Color.white;
                outline.OutlineWidth = outlineWidth;
            }
            outline.enabled = true;
        }
        else
        {
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (orbitCenter != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(orbitCenter.position, targetOrbitRadius);
        }
    }
}
