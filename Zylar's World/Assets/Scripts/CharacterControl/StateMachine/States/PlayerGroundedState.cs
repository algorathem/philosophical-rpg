using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{
    private SlopeData slopeData;
    public PlayerGroundedState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
        slopeData = stateMachine.player.colliderUtility.slopeData;
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.player.animationData.GroundedParameterHash);

        UpdateShouldSprintState();

        UpdateCameraRecenteringState(stateMachine.ReusableData.movementInput);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.player.animationData.GroundedParameterHash);
    }



    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Float();
    }

    private void Float()
    {
        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.player.colliderUtility.capsuleColliderData.collider.bounds.center;
        Ray downwardRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        if (Physics.Raycast(downwardRayFromCapsuleCenter, out RaycastHit hit, slopeData.floatRayDistance, stateMachine.player.layerData.groundLayer, QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardRayFromCapsuleCenter.direction);
            float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);
            if (slopeSpeedModifier == 0f)
            {
                return;
            }

            float distanceToFloatingPoint = stateMachine.player.colliderUtility.capsuleColliderData.colliderCenterInLocalSpace.y * stateMachine.player.transform.localScale.y - hit.distance;
            if (distanceToFloatingPoint == 0f)
            {
                return;
            }
            float amountToLift = distanceToFloatingPoint * slopeData.stepReachForce - GetPlayerVerticalVelocity().y;
            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);
            stateMachine.player.rb.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }

    protected virtual void OnMove()
    {
        if (stateMachine.ReusableData.shouldSprint)
        {
            stateMachine.ChangeState(stateMachine.sprintingState);
            return;
        }

        if (stateMachine.ReusableData.shouldWalk)
        {
            stateMachine.ChangeState(stateMachine.walkingState);
            return;
        }

        stateMachine.ChangeState(stateMachine.runningState);
    }

    protected override void OnContactWithGroundExited(Collider collider)
    {
        base.OnContactWithGroundExited(collider);

        if (IsThereGroundUnderneath())
        {
            return;
        }

        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.player.colliderUtility.capsuleColliderData.collider.bounds.center;
        Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - stateMachine.player.colliderUtility.capsuleColliderData.colliderVerticalExtents, Vector3.down);

        if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, movementData.groundToFallRayDistance, stateMachine.player.layerData.groundLayer, QueryTriggerInteraction.Ignore))
        {
            OnFall();
        }


    }

    protected virtual void OnFall()
    {
        stateMachine.ChangeState(stateMachine.fallingState);
    }

    private float SetSlopeSpeedModifierOnAngle(float groundAngle)
    {
        float slopeSpeedModifier = movementData.slopeSpeedAngle.Evaluate(groundAngle);
        if (stateMachine.ReusableData.movementOnSlopeSpeedMultiplier != slopeSpeedModifier)
        {
            stateMachine.ReusableData.movementOnSlopeSpeedMultiplier = slopeSpeedModifier;

            UpdateCameraRecenteringState(stateMachine.ReusableData.movementInput);
        }
        return slopeSpeedModifier;
    }

    private void UpdateShouldSprintState()
    {
        if (!stateMachine.ReusableData.shouldSprint)
        {
            return;
        }
        if (stateMachine.ReusableData.movementInput != Vector2.zero)
        {
            return;
        }
        stateMachine.ReusableData.shouldSprint = false;
    }

    private bool IsThereGroundUnderneath()
    {
        BoxCollider groundCheckCollider = stateMachine.player.colliderUtility.triggercolliderData.groundCheckCollider;
        Vector3 groundColliderCenterInWorldSpace = groundCheckCollider.bounds.center;
        Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderCenterInWorldSpace, stateMachine.player.colliderUtility.triggercolliderData.groundCheckColliderExtents, groundCheckCollider.transform.rotation, stateMachine.player.layerData.groundLayer, QueryTriggerInteraction.Ignore);

        return overlappedGroundColliders.Length > 0;
    }

    protected override void AddInputActionsCallback()
    {
        base.AddInputActionsCallback();

        stateMachine.player.playerInput.PlayerActions.Dash.started += OnDashStarted;

        stateMachine.player.playerInput.PlayerActions.Jump.started += OnJumpStarted;
    }



    protected override void RemoveInputActionsCallback()
    {
        base.RemoveInputActionsCallback();

        stateMachine.player.playerInput.PlayerActions.Dash.started -= OnDashStarted;

        stateMachine.player.playerInput.PlayerActions.Jump.started -= OnJumpStarted;
    }


    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);

        stateMachine.ChangeState(stateMachine.runningState);
    }

    protected virtual void OnDashStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.dashingState);
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.jumpingState);
    }

}
