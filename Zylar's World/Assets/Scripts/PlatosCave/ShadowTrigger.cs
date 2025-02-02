using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTrigger : MonoBehaviour
{
    [SerializeField] private GameObject shadowOuterObject;
    public bool isTriggered { get; set; } = false;
    private int timer = 0;

    private void Update()
    {
        if (timer > 1)
        {
            isTriggered = false;
            timer = 0;
        }
        else
        {
            timer++;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            isTriggered = true;
        }
    }


}
