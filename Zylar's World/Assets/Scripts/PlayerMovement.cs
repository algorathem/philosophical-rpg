using UnityEngine;

public class PlayerMovemen : MonoBehaviour
{

    const float defaultSpeed = 7f;
    const float runningSpeed = 14f;
    const float crouchSpeed = 3f;
    [SerializeField] private float playerSpeed = defaultSpeed;
    [SerializeField] private float jumpingForce = 6;

    private bool isOnGround = true;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 inputVector = new Vector3(0, 0, 0);
        Vector3 scaleVec = new Vector3(1f, 1f, 1f);

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
        }

        // Jump Logic
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(Vector3.up * jumpingForce, ForceMode.Impulse);
            isOnGround = false;
        }

        inputVector = inputVector.normalized;
        transform.position += inputVector * playerSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }
}
