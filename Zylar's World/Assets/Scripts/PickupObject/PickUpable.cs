using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpable : MonoBehaviour
{
    bool isHolding = false;
    [SerializeField] float throwForce = 10;
    [SerializeField] float maxDistance = 5;
    [SerializeField] float distance;

    TempParent tempParent;
    Rigidbody rb;
    Vector3 objectPos;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tempParent = TempParent.Instance;
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.uiUtility.isAiming)
        {
            if (isHolding)
            {
                Drop();
            }
        }
        if (isHolding)
        {
            Hold();
        }
    }

    private void OnMouseDown()
    {
        if (tempParent != null)
        {
            distance = Vector3.Distance(this.transform.position, tempParent.transform.position);
            if (distance <= maxDistance)
            {
                isHolding = true;
                rb.useGravity = false;
                rb.detectCollisions = true;

                this.transform.SetParent(tempParent.transform);
            }

        }
        else
        {
            Debug.Log("TempParent is null");
        }
    }

    private void OnMouseUp()
    {
        Drop();
    }

    private void OnMouseExit()
    {
        Drop();
    }

    private void Hold()
    {
        distance = Vector3.Distance(this.transform.position, tempParent.transform.position);

        if (distance >= maxDistance)
        {
            Drop();
        }

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void Drop()
    {
        if (isHolding)
        {
            isHolding = false;
            objectPos = this.transform.position;
            this.transform.position = objectPos;
            this.transform.SetParent(null);
            rb.useGravity = true;
        }
    }

}
