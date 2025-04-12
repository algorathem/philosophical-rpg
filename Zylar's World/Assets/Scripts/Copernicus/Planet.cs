using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float rotationSpeed;
    public float dampAmount;

    private bool isDragging = false;
    private Vector3 offset;

    void Update()
    {
        if (!isDragging)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime * dampAmount, Space.Self);
    }

    private void OnMouseDown()
    {
        isDragging = true;

        // Store offset between mouse and object position
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            offset = transform.position - ray.GetPoint(distance);
        }
    }

    private void OnMouseDrag()
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance);
            transform.position = point + offset;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        // Check for orbit snap (next step)
        PlanetSnapZone snapZone = FindClosestOrbit();
        if (snapZone != null && snapZone.IsInsideSnapZone(transform.position))
        {
            transform.position = snapZone.snapPoint.position;
            // Optionally: trigger visual feedback or lock interaction
        }
    }

    private PlanetSnapZone FindClosestOrbit()
    {
        PlanetSnapZone[] zones = FindObjectsOfType<PlanetSnapZone>();
        PlanetSnapZone best = null;
        float closest = Mathf.Infinity;
        foreach (var z in zones)
        {
            float dist = Vector3.Distance(transform.position, z.snapPoint.position);
            if (dist < closest)
            {
                best = z;
                closest = dist;
            }
        }
        return best;
    }
}

