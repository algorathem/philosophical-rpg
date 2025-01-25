using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintingState : PlayerMovingState
{
    private PlayerSprintData sprintData;
    private bool keepSprinting;
    private bool shouldResetSprintState;
    private float startTime;
    public PlayerSprintingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
        sprintData = movementData.sprintData;
    }

    public override void Enter()
    {
        stateMachine.ReusableData.movementSpeedMultiplier = sprintData.speedMultiplier;
        base.Enter();
        StartAnimation(stateMachine.player.animationData.SprintParameterHash);

        stateMachine.ReusableData.currentJumpForce = airborneData.jumpData.strongForce;

        shouldResetSprintState = true;

        startTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        if (shouldResetSprintState)
        {
            keepSprinting = false;
            stateMachine.ReusableData.shouldSprint = false;
        }
        StopAnimation(stateMachine.player.animationData.SprintParameterHash);


    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (keepSprinting)
        {
            return;
        }

        if (Time.time < startTime + sprintData.sprintToRunTime)
        {
            return;
        }

        StopSprinting();
    }

    private void StopSprinting()
    {
        if (stateMachine.ReusableData.movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.idlingState);
            return;
        }

        stateMachine.ChangeState(stateMachine.runningState);
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.hardStoppingState);

        base.OnMovementCanceled(context);
    }

    protected override void AddInputActionsCallback()
    {
        base.AddInputActionsCallback();
        stateMachine.player.playerInput.PlayerActions.Sprint.performed += OnSprintPerformed;
    }

    protected override void RemoveInputActionsCallback()
    {
        base.RemoveInputActionsCallback();
        stateMachine.player.playerInput.PlayerActions.Sprint.performed -= OnSprintPerformed;
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        keepSprinting = true;

        stateMachine.ReusableData.shouldSprint = true;
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        shouldResetSprintState = false;
        base.OnJumpStarted(context);
    }

    protected override void OnFall()
    {
        shouldResetSprintState = false;
        base.OnFall();
    }

}
