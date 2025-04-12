using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSnapZone : MonoBehaviour
{
    public Transform snapPoint; // Position where planet should be snapped to
    public float snapRadius = 1f; // How close the planet needs to be to snap

    public bool IsInsideSnapZone(Vector3 position)
    {
        return Vector3.Distance(position, snapPoint.position) <= snapRadius;
    }

    private void OnDrawGizmos()
    {
        if (snapPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(snapPoint.position, snapRadius);
        }
    }
}

