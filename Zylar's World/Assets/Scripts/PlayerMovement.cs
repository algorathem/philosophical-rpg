using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    const float defaultSpeed = 7f;
    const float runningSpeed = 14f;
    const float crouchSpeed = 3f;
    [SerializeField] private float playerSpeed = defaultSpeed;
    [SerializeField] private float jumpingForce = 7f; // Adjust for desired jump height
    [SerializeField] private float fallMultiplier = 2.5f; // Speed up fall
    [SerializeField] private float lowJumpMultiplier = 2f; // Faster low jump descent
    [SerializeField] private float rotationSpeed = 100f;

    private bool isOnGround = true;

    Rigidbody rb;

    public Transform cameraTransform; // Assign the camera transform in the Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 inputVector = new Vector3(0, 0, 0);
        Vector3 scaleVec = new Vector3(1f, 1f, 1f);

        //Check if player is offworld
        OffWorld();

        // Movement logic
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.z += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x += -1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.z += -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x += 1;
        }

        // Running logic
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerSpeed = runningSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerSpeed = defaultSpeed;
        }

        /*
        // Crouch Logic
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerSpeed = crouchSpeed;
            scaleVec.y = 0.5f;
            transform.localScale = scaleVec;
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            playerSpeed = defaultSpeed;
            scaleVec.y = 1f;
            transform.localScale = scaleVec;
        }*/

        // Jump Logic
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpingForce, rb.velocity.z);
            isOnGround = false;
        }

        // Modify jump arc for natural feel
        if (rb.velocity.y < 0)
        {
            // Increase gravity for faster falling
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            // Apply low jump multiplier if jump button is released early
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Turning logic using Q and E
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0); // Rotate left
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0); // Rotate right
        }

        // Camera follows player's direction
        if (cameraTransform != null)
        {
            Vector3 cameraOffset = new Vector3(0, 5, -10);
            cameraTransform.position = transform.position + transform.rotation * cameraOffset;
            cameraTransform.LookAt(transform.position + Vector3.up * 1.5f);
        }

        // Normalize movement vector and apply movement
        inputVector = inputVector.normalized;
        transform.position += transform.TransformDirection(inputVector) * playerSpeed * Time.deltaTime;

        // Ensure the player stays upright
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.x = 0; // Lock X-axis rotation
        currentRotation.z = 0; // Lock Z-axis rotation
        transform.eulerAngles = currentRotation;
    }

    private void FixedUpdate()
    {
        // Ensure ground collision is detected properly
        if (Physics.Raycast(transform.position, Vector3.down, 1.1f))
        {
            isOnGround = true;
        }
    }

    private void OffWorld()
    {
        if (transform.position.y < -10)
        {
            transform.position = Vector3.zero;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }
}
