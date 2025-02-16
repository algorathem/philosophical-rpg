using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    const float defaultSpeed = 7f;
    const float runningSpeed = 14f;
    const float crouchSpeed = 3f;

    [SerializeField] private float playerSpeed = defaultSpeed;
    [SerializeField] private float jumpingForce = 6;
    [SerializeField] private float dragForce = 4;
    [SerializeField] private Transform orientation;

    private bool isOnGround = true;
    float horizontalInput;
    float verticalInput;

    Rigidbody rb;
    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();
        speedControl();

        rb.drag = dragForce;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void InputHandler() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer() { 
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * playerSpeed, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    private void speedControl()
    {
        Vector3 flatvel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatvel.magnitude > playerSpeed)
        {
            Vector3 newSpeed = flatvel.normalized * playerSpeed;
            rb.velocity = new Vector3(newSpeed.x, rb.velocity.y, newSpeed.z);
        }
    }
}
