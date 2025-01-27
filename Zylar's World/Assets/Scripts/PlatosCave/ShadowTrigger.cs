using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTrigger : MonoBehaviour
{
    [SerializeField] private GameObject shadowOuterObject;
    public bool isTriggered { get; set; } = false;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == shadowOuterObject && !isTriggered)
        {
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == shadowOuterObject)
        {
            isTriggered = false;
        }
    }
}
