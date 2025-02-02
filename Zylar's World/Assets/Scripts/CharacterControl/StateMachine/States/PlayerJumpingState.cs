using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpingState : PlayerAirborneState
{
    private PlayerJumpData jumpData;
    private bool shouldKeepRotating;
    private bool canStartFalling;
    public PlayerJumpingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
        jumpData = airborneData.jumpData;
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.ReusableData.movementSpeedMultiplier = 0f;
        stateMachine.ReusableData.rotationData = jumpData.rotationData;
        stateMachine.ReusableData.movementDecelerationForce = jumpData.decelerationForce;
        shouldKeepRotating = stateMachine.ReusableData.movementInput != Vector2.zero;
        Jump();
    }

    public override void Exit()
    {
        base.Exit();
        SetBaseRotationData();
        canStartFalling = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!canStartFalling && IsMovingUp(0f))
        {
            canStartFalling = true;
        }

        if (!canStartFalling || GetPlayerVerticalVelocity().y > 0)
        {
            return;
        }

        stateMachine.ChangeState(stateMachine.fallingState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (shouldKeepRotating)
        {
            RotateTowardsTargetRotation();
        }

        if (IsMovingUp())
        {
            DecelerateVertically();
        }
    }

    protected override void ResetSprintState()
    {
    }

    private void Jump()
    {
        Vector3 jumpForce = stateMachine.ReusableData.currentJumpForce;

        // Apply direction to jump force
        Vector3 jumpDirection = stateMachine.player.transform.forward;

        if (shouldKeepRotating)
        {
            UpdateTargetRotation(GetMovementInputDirection());
            jumpDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);
        }

        jumpForce.x *= jumpDirection.x;
        jumpForce.z *= jumpDirection.z;

        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.player.colliderUtility.capsuleColliderData.collider.bounds.center;
        Ray downwardRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);
        if (Physics.Raycast(downwardRayFromCapsuleCenter, out RaycastHit hit, jumpData.jumpToGroundRayDistance, stateMachine.player.layerData.groundLayer, QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardRayFromCapsuleCenter.direction);

            if (IsMovingUp())
            {
                float forceModifier = jumpData.jumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);

                jumpForce.x *= forceModifier;
                jumpForce.z *= forceModifier;
            }

            if (IsMovingDown())
            {
                float forceModifier = jumpData.jumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);

                jumpForce.y *= forceModifier;
            }
        }

        ResetVelocity(); // So velocity does not affect jump

        stateMachine.player.rb.AddForce(jumpForce, ForceMode.VelocityChange);
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
    }

}
