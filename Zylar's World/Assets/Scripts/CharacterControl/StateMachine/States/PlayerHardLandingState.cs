using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHardLandingState : PlayerLandingState
{
    public PlayerHardLandingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ReusableData.movementSpeedMultiplier = 0f;
        base.Enter();
        StartAnimation(stateMachine.player.animationData.HardLandParameterHash);

        stateMachine.player.playerInput.PlayerActions.Movement.Disable();
        ResetVelocity();
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.player.playerInput.PlayerActions.Movement.Enable();
        StopAnimation(stateMachine.player.animationData.HardLandParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!IsMovingHorizontally())
        {
            return;
        }

        ResetVelocity();
    }

    public override void OnAnimationExitEvent()
    {
        stateMachine.player.playerInput.PlayerActions.Movement.Enable();
    }

    public override void OnAnimationTransitionEvent()
    {
        stateMachine.ChangeState(stateMachine.idlingState);
    }

    protected override void AddInputActionsCallback()
    {
        base.AddInputActionsCallback();
        stateMachine.player.playerInput.PlayerActions.Movement.started += OnMovementStarted;
    }

    protected override void RemoveInputActionsCallback()
    {
        base.RemoveInputActionsCallback();
        stateMachine.player.playerInput.PlayerActions.Movement.started -= OnMovementStarted;
    }

    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        OnMove();
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
    }

    protected override void OnMove()
    {
        if (stateMachine.ReusableData.shouldWalk)
        {
            return;
        }

        stateMachine.ChangeState(stateMachine.runningState);
    }

}
