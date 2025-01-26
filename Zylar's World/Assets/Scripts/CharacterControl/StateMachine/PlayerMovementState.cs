using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementState : IState
{
    protected PlayerMovementStateMachine stateMachine;

    protected PlayerGroundedData movementData;
    protected PlayerAirborneData airborneData;

    public PlayerMovementState(PlayerMovementStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        movementData = stateMachine.player.Data.groundedData;
        airborneData = stateMachine.player.Data.airborneData;

        SetBaseCameraRecenteringData();

        InitialiseData();
    }

    protected void SetBaseCameraRecenteringData()
    {
        stateMachine.ReusableData.backwardsCameraRecenteringData = movementData.backwardsCameraRecenteringData;
        stateMachine.ReusableData.sidewaysCameraRecenteringData = movementData.sidewaysCameraRecenteringData;

    }

    private void InitialiseData()
    {
        SetBaseRotationData();
    }

    public virtual void Enter()
    {
        Debug.Log("State: " + this.GetType().Name);

        AddInputActionsCallback();
    }

    public virtual void Exit()
    {
        Debug.Log("Exiting State: " + this.GetType().Name);

        RemoveInputActionsCallback();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void LogicUpdate()
    {
    }

    public virtual void PhysicsUpdate()
    {
        Move();
    }

    public virtual void UiUpdate()
    {
        // Check if camera target has reached the target position
        if (Vector3.Distance(stateMachine.player.cameraUtility.cameraTarget.localPosition, stateMachine.player.cameraUtility.finalTargetPosition) < stateMachine.player.cameraUtility.targetReachThreshold)
        {
            if (stateMachine.player.uiUtility.ShouldDisplayAimCursor())
            {
                stateMachine.player.uiUtility.EnableAimCursor();
            }
            if (stateMachine.player.uiUtility.ShouldDisplaySelectCursor())
            {
                stateMachine.player.uiUtility.EnableSelectCursor();
            }
        }
    }

    public virtual void OnAnimationEnterEvent()
    {
    }

    public virtual void OnAnimationExitEvent()
    {
    }

    public virtual void OnAnimationTransitionEvent()
    {
    }

    public virtual void OnTriggerEnter(Collider collider)
    {
        if (stateMachine.player.layerData.IsGroundLayer(collider.gameObject.layer))
        {
            OnContactWithGround(collider);

            return;
        }
    }

    public virtual void OnTriggerExit(Collider collider)
    {
        if (stateMachine.player.layerData.IsGroundLayer(collider.gameObject.layer))
        {
            OnContactWithGroundExited(collider);
            return;
        }
    }



    // Function to read the movement input from the player
    private void ReadMovementInput()
    {
        stateMachine.ReusableData.movementInput = stateMachine.player.playerInput.PlayerActions.Movement.ReadValue<Vector2>();
    }

    // Function to move the player
    private void Move()
    {
        // If the player is not moving or the speed multiplier is 0, return
        if (stateMachine.ReusableData.movementInput == Vector2.zero || stateMachine.ReusableData.movementSpeedMultiplier == 0f)
        {
            return;
        }

        // Get the movement direction and speed
        Vector3 moveDirection = GetMovementInputDirection();

        // Rotate the player
        float targetRotationYAngle = Rotate(moveDirection);

        // Get the target rotation direction
        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

        float movementSpeed = GetMovementSpeed();

        Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

        // Move the player
        stateMachine.player.rb.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
    }

    protected Vector3 GetTargetRotationDirection(float targetRotationYAngle)
    {
        return Quaternion.Euler(0f, targetRotationYAngle, 0f) * Vector3.forward;
    }

    private float Rotate(Vector3 direction)
    {
        float directionAngle = UpdateTargetRotation(direction);

        RotateTowardsTargetRotation();

        return directionAngle;
    }

    private void UpdateTargetRotationData(float newDirectionAngle)
    {
        stateMachine.ReusableData.CurrentTargetRotation.y = newDirectionAngle;
        stateMachine.ReusableData.DampedTargetRotationPassedTime.y = 0f;
    }

    // Function to get the player's horizontal velocity
    protected Vector3 GetPlayerHorizontalVelocity()
    {
        Vector3 horizontalVelocity = stateMachine.player.rb.velocity;
        horizontalVelocity.y = 0f;
        return horizontalVelocity;
    }

    // Function to get the player's vertical velocity
    protected Vector3 GetPlayerVerticalVelocity()
    {
        return new Vector3(0f, stateMachine.player.rb.velocity.y, 0f);
    }

    // Function to get the movement speed
    protected float GetMovementSpeed(bool shouldConsiderSlope = true)
    {
        float movementSpeed = movementData.baseSpeed * stateMachine.ReusableData.movementSpeedMultiplier;

        if (shouldConsiderSlope)
        {
            movementSpeed *= stateMachine.ReusableData.movementOnSlopeSpeedMultiplier;
        }

        return movementSpeed;
    }

    // Function to get the movement input direction
    protected Vector3 GetMovementInputDirection()
    {
        return new Vector3(stateMachine.ReusableData.movementInput.x, 0f, stateMachine.ReusableData.movementInput.y);
    }

    // Function to rotate the player towards the target rotation
    protected void RotateTowardsTargetRotation()
    {
        float currentYAngle = stateMachine.player.transform.eulerAngles.y;

        if (currentYAngle == stateMachine.ReusableData.CurrentTargetRotation.y)
        {
            return;
        }

        float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, stateMachine.ReusableData.CurrentTargetRotation.y, ref stateMachine.ReusableData.DampedTargetRotationCurrentVelocity.y, stateMachine.ReusableData.TimeToReachTargetRotation.y - stateMachine.ReusableData.DampedTargetRotationPassedTime.y);
        stateMachine.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

        Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);
        stateMachine.player.rb.MoveRotation(targetRotation);
    }

    protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
    {
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        if (directionAngle < 0f)
        {
            directionAngle += 360f;
        }

        // Add the camera's rotation to the direction angle
        if (shouldConsiderCameraRotation)
        {
            directionAngle += stateMachine.player.MainCameraTransform.eulerAngles.y;
            if (directionAngle > 360f)
            {
                directionAngle -= 360f;
            }
        }


        if (directionAngle != stateMachine.ReusableData.CurrentTargetRotation.y)
        {
            UpdateTargetRotationData(directionAngle);
        }

        return directionAngle;
    }

    protected void ResetVelocity()
    {
        stateMachine.player.rb.velocity = Vector3.zero;
    }

    protected void ResetVerticalVelocity()
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
        stateMachine.player.rb.velocity = playerHorizontalVelocity;
    }

    protected virtual void OnContactWithGround(Collider collider)
    {

    }

    protected virtual void OnContactWithGroundExited(Collider collider)
    {

    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        DisableCameraRecentering();
    }

    protected virtual void AddInputActionsCallback()
    {
        stateMachine.player.playerInput.PlayerActions.WalkToggle.started += OnWalkToggleStarted;
        stateMachine.player.playerInput.PlayerActions.Look.started += OnMouseMovementStarted;
        stateMachine.player.playerInput.PlayerActions.Movement.performed += OnMovementPerformed;
        stateMachine.player.playerInput.PlayerActions.Movement.canceled += OnMovementCanceled;

        // Aim action
        stateMachine.player.playerInput.PlayerActions.Aim.performed += OnAimPerformed;
        stateMachine.player.playerInput.PlayerActions.Aim.canceled += OnAimCanceled;

        // Select action
        stateMachine.player.playerInput.PlayerActions.Select.performed += OnSelectPerformed;
        stateMachine.player.playerInput.PlayerActions.Select.canceled += OnSelectCanceled;

    }

    protected virtual void RemoveInputActionsCallback()
    {
        stateMachine.player.playerInput.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;
        stateMachine.player.playerInput.PlayerActions.Look.started -= OnMouseMovementStarted;
        stateMachine.player.playerInput.PlayerActions.Movement.performed -= OnMovementPerformed;
        stateMachine.player.playerInput.PlayerActions.Movement.canceled -= OnMovementCanceled;

        // Aim action
        stateMachine.player.playerInput.PlayerActions.Aim.performed -= OnAimPerformed;
        stateMachine.player.playerInput.PlayerActions.Aim.canceled -= OnAimCanceled;

        // Select action
        stateMachine.player.playerInput.PlayerActions.Select.performed -= OnSelectPerformed;
        stateMachine.player.playerInput.PlayerActions.Select.canceled -= OnSelectCanceled;
    }

    protected virtual void OnSelectPerformed(InputAction.CallbackContext context)
    {
        if (!stateMachine.player.uiUtility.isAiming || !stateMachine.player.uiUtility.isAimCursorEnabled)
        {
            return;
        }
        Debug.Log("Left mouse button pressed (Select started)");

        // Enable select cursor
        stateMachine.player.uiUtility.isSelecting = true;

        // Disable aim cursor
        stateMachine.player.uiUtility.DisableAimCursor();

    }

    protected virtual void OnSelectCanceled(InputAction.CallbackContext context)
    {
        if (!stateMachine.player.uiUtility.isAiming)
        {
            return;
        }
        Debug.Log("Left mouse button released (Select canceled)");

        // Disable select cursor
        stateMachine.player.uiUtility.isSelecting = false;
        stateMachine.player.uiUtility.DisableSelectCursor();
    }

    protected virtual void OnAimPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Right mouse button pressed (Aim started)");

        // Move camera target upwards by aim offset
        stateMachine.player.cameraUtility.cameraTarget.localPosition = stateMachine.player.cameraUtility.finalTargetPosition;

        // Enable aim cursor
        stateMachine.player.uiUtility.isAiming = true;

        // Disable select cursor
        stateMachine.player.uiUtility.DisableSelectCursor();
    }

    protected virtual void OnAimCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Right mouse button released (Aim canceled)");

        // Reset camera target position
        stateMachine.player.cameraUtility.cameraTarget.localPosition = stateMachine.player.cameraUtility.originalTargetPosition;

        // Disable select cursor
        stateMachine.player.uiUtility.DisableSelectCursor();

        // Disable aim cursor
        stateMachine.player.uiUtility.isAiming = false;
        stateMachine.player.uiUtility.DisableAimCursor();

    }

    protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        stateMachine.ReusableData.shouldWalk = !stateMachine.ReusableData.shouldWalk;
    }

    private void OnMouseMovementStarted(InputAction.CallbackContext context)
    {
        UpdateCameraRecenteringState(stateMachine.ReusableData.movementInput);
    }

    protected virtual void OnMovementPerformed(InputAction.CallbackContext context)
    {
        UpdateCameraRecenteringState(context.ReadValue<Vector2>());
    }

    protected void DecelerateHorizontally()
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
        stateMachine.player.rb.AddForce(-playerHorizontalVelocity * stateMachine.ReusableData.movementDecelerationForce, ForceMode.Acceleration);
    }

    protected void DecelerateVertically()
    {
        Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();
        stateMachine.player.rb.AddForce(-playerVerticalVelocity * stateMachine.ReusableData.movementDecelerationForce, ForceMode.Acceleration);
    }

    protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
        Vector2 playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x, playerHorizontalVelocity.z);
        return playerHorizontalMovement.magnitude > minimumMagnitude;
    }

    protected bool IsMovingUp(float minimumVelocity = 0.1f)
    {
        return GetPlayerVerticalVelocity().y > minimumVelocity;
    }

    protected bool IsMovingDown(float minimumVelocity = 0.1f)
    {
        return GetPlayerVerticalVelocity().y < -minimumVelocity;
    }

    protected void SetBaseRotationData()
    {
        stateMachine.ReusableData.rotationData = movementData.baseRotationData;
        stateMachine.ReusableData.TimeToReachTargetRotation = stateMachine.ReusableData.rotationData.targetRotationReachTime;

    }

    protected void EnableCameraRecentering(float waitTime = -1f, float recenteringTime = -1f)
    {
        float movementSpeed = GetMovementSpeed();

        if (movementSpeed == 0f)
        {
            movementSpeed = movementData.baseSpeed;
        }

        stateMachine.player.cameraUtility.EnableRecentering(waitTime, recenteringTime, movementData.baseSpeed, movementSpeed);
    }

    protected void DisableCameraRecentering()
    {
        stateMachine.player.cameraUtility.DisableRecentering();
    }

    protected void UpdateCameraRecenteringState(Vector2 movementInput)
    {
        if (movementInput == Vector2.zero)
        {
            return;
        }

        if (movementInput == Vector2.up)
        {
            DisableCameraRecentering();

            return;
        }

        float cameraVerticalAngle = stateMachine.player.MainCameraTransform.eulerAngles.x;

        if (cameraVerticalAngle >= 270f)
        {
            cameraVerticalAngle -= 360f;
        }

        cameraVerticalAngle = Mathf.Abs(cameraVerticalAngle);

        if (movementInput == Vector2.down)
        {
            SetCameraRecenteringState(cameraVerticalAngle, stateMachine.ReusableData.backwardsCameraRecenteringData);


            return;
        }

        SetCameraRecenteringState(cameraVerticalAngle, stateMachine.ReusableData.sidewaysCameraRecenteringData);
    }

    protected void SetCameraRecenteringState(float cameraVerticalAngle, List<PlayerCameraRecenteringData> cameraRecenteringData)
    {
        foreach (PlayerCameraRecenteringData recenteringData in cameraRecenteringData)
        {
            if (!recenteringData.IsWithinRange(cameraVerticalAngle))
            {
                continue;
            }

            EnableCameraRecentering(recenteringData.waitTime, recenteringData.recenteringTime);

            return;
        }

        DisableCameraRecentering();
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.player.animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.player.animator.SetBool(animationHash, false);
    }

}
