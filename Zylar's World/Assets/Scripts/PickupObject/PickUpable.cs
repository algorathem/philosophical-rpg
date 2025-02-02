using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpable : MonoBehaviour
{
    bool isHolding = false;
    [SerializeField] float maxDistance = 5;
    [SerializeField] float distance;
    [SerializeField] bool useGravity = true;
    [SerializeField] public bool isMovable { get; set; } = true;

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

        if (!useGravity)
        {
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
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
            Rotate();
        }
    }

    private void OnMouseEnter()
    {
        if (!isMovable)
        {
            return;
        }

        if (tempParent != null)
        {
            distance = Vector3.Distance(this.transform.position, tempParent.transform.position);
            if (distance <= maxDistance)
            {
                player.uiUtility.isHovering = true;
                player.uiUtility.DisableAimCursor();
            }
        }
    }

    private void OnMouseDown()
    {
        if (!isMovable)
        {
            return;
        }

        // Main logic for picking up the object
        if (tempParent != null)
        {
            distance = Vector3.Distance(this.transform.position, tempParent.transform.position);
            if (distance <= maxDistance)
            {
                isHolding = true;
                if (useGravity)
                {
                    rb.useGravity = false;
                }
                rb.detectCollisions = true;

                this.transform.SetParent(tempParent.transform);

                // UI logic
                player.uiUtility.isSelecting = true;
                player.uiUtility.DisableHoverCursor();
                player.uiUtility.EnableSelectCursor();
            }

        }
        else
        {
            Debug.Log("TempParent is null");
        }
    }

    private void OnMouseUp()
    {
        player.uiUtility.isSelecting = false;
        player.uiUtility.DisableSelectCursor();
        player.uiUtility.EnableHoverCursor();
        Drop();
    }

    private void OnMouseExit()
    {
        player.uiUtility.DisableHoverCursor();
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
            if (useGravity)
            {
                rb.useGravity = true;
            }
        }
    }

    private void Rotate()
    {
        if (Input.GetKey(KeyCode.E))
        {
            // Rotate the object to the right
            this.transform.Rotate(Vector3.up, Space.World);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            // Rotate the object to the up
            this.transform.Rotate(Vector3.left, Space.World);
        }
    }

}
