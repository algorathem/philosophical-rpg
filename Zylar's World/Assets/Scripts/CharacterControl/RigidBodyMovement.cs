using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    private Rigidbody rb;
    private bool isGrounded = true; // Check if the player is on the ground

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        // Check for the Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");  // A/D or Left/Right Arrow
        float moveZ = Input.GetAxis("Vertical");    // W/S or Up/Down Arrow

        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;

        // Apply movement to the Rigidbody
        rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        // Apply upward force for jumping
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; // Prevent multiple jumps mid-air
    }

    // Detect if the player is on the ground
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Allow jumping again
        }
    }
}


