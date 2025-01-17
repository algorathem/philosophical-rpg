using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");  // A/D or Left/Right Arrow
        float moveZ = Input.GetAxis("Vertical");    // W/S or Up/Down Arrow

        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;

        // Apply movement to the Rigidbody
        rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);
    }
}
