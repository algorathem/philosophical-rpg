using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowObject : MonoBehaviour
{
    [SerializeField] private GameObject shadowObject;
    public bool isShadowed { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == shadowObject)
        {
            isShadowed = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == shadowObject)
        {
            isShadowed = false;
        }
    }
}
