using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public PlayerSO Data { get; private set; }

    [field: Header("Collisions")]
    [field: SerializeField] public PlayerCapsuleColliderUtility colliderUtility { get; private set; }
    [field: SerializeField] public PlayerLayerData layerData { get; private set; }

    [field: Header("Camera")]
    [field: SerializeField] public PlayerCameraUtility cameraUtility { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData animationData { get; private set; }

    public Rigidbody rb { get; private set; }
    public Animator animator { get; private set; }
    public Transform MainCameraTransform { get; private set; }
    public PlayerInput playerInput { get; private set; }
    private PlayerMovementStateMachine movementStateMachine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();  // Get the Rigidbody component
        animator = GetComponentInChildren<Animator>();  // Get the Animator component
        movementStateMachine = new PlayerMovementStateMachine(this);  // Create a new instance of the PlayerMovementStateMachine class
        playerInput = GetComponent<PlayerInput>();  // Get the PlayerInput component

        colliderUtility.Initialize(gameObject);  // Initialize the CapsuleColliderUtility component
        colliderUtility.CalculateCapsuleColliderDimension();  // Calculate the CapsuleCollider dimension
        cameraUtility.Initialize();  // Initialize the PlayerCameraUtility component
        animationData.Initialize();  // Initialize the PlayerAnimationData component

        MainCameraTransform = Camera.main.transform;  // Get the main camera's transform
    }

    private void OnValidate()
    {
        colliderUtility.Initialize(gameObject);  // Initialize the CapsuleColliderUtility component
        colliderUtility.CalculateCapsuleColliderDimension();  // Calculate the CapsuleCollider dimension
    }

    private void Start()
    {
        movementStateMachine.ChangeState(movementStateMachine.idlingState);
    }

    private void OnTriggerEnter(Collider collider)
    {
        movementStateMachine.OnTriggerEnter(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        movementStateMachine.OnTriggerExit(collider);
    }

    private void Update()
    {
        movementStateMachine.HandleInput();
        movementStateMachine.LogicUpdate();
    }

    private void FixedUpdate()
    {
        movementStateMachine.PhysicsUpdate();
    }

    public void OnMovementStateAnimationEnterEvent()
    {
        movementStateMachine.OnAnimationEnterEvent();
    }

    public void OnMovementStateAnimationExitEvent()
    {
        movementStateMachine.OnAnimationExitEvent();
    }

    public void OnMovementStateAnimationTransitionEvent()
    {
        movementStateMachine.OnAnimationTransitionEvent();
    }
}
