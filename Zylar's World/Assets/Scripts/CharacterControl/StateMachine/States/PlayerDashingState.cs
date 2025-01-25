using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashingState : PlayerGroundedState
{
    private PlayerDashData dashData;
    private bool shouldKeepRotating;
    public PlayerDashingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
        dashData = movementData.dashData;
    }

    public override void Enter()
    {
        stateMachine.ReusableData.movementSpeedMultiplier = dashData.speedMultiplier;
        base.Enter();
        StartAnimation(stateMachine.player.animationData.DashParameterHash);

        stateMachine.ReusableData.currentJumpForce = airborneData.jumpData.strongForce;
        stateMachine.ReusableData.rotationData = dashData.rotationData;
        Dash();
        shouldKeepRotating = stateMachine.ReusableData.movementInput != Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.player.animationData.DashParameterHash);

        SetBaseRotationData();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!shouldKeepRotating)
        {
            return;
        }
        RotateTowardsTargetRotation();
    }

    public override void OnAnimationTransitionEvent()
    {
        if (stateMachine.ReusableData.movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.hardStoppingState);
            return;
        }

        stateMachine.ChangeState(stateMachine.sprintingState);
    }

    protected override void AddInputActionsCallback()
    {
        base.AddInputActionsCallback();
        stateMachine.player.playerInput.PlayerActions.Movement.performed += OnMovementPerformed;
    }

    protected override void RemoveInputActionsCallback()
    {
        base.RemoveInputActionsCallback();
        stateMachine.player.playerInput.PlayerActions.Movement.performed -= OnMovementPerformed;
    }

    private void Dash()
    {
        Vector3 dashDirection = stateMachine.player.transform.forward;
        dashDirection.y = 0f;
        UpdateTargetRotation(dashDirection, false);

        if (stateMachine.ReusableData.movementInput != Vector2.zero)
        {
            UpdateTargetRotation(GetMovementInputDirection());

            dashDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);
        }


        stateMachine.player.rb.velocity = dashDirection * GetMovementSpeed(false);
    }

    protected override void OnDashStarted(InputAction.CallbackContext context)
    {
    }

    protected override void OnMovementPerformed(InputAction.CallbackContext context)
    {
        base.OnMovementPerformed(context);
        shouldKeepRotating = true;
    }
}
