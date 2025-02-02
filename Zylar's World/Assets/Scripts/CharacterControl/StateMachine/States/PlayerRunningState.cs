using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunningState : PlayerMovingState
{
    private float startTime;
    private PlayerSprintData sprintData;
    public PlayerRunningState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
        sprintData = movementData.sprintData;
    }

    public override void Enter()
    {
        stateMachine.ReusableData.movementSpeedMultiplier = movementData.runData.speedMultiplier;
        base.Enter();
        stateMachine.ReusableData.currentJumpForce = airborneData.jumpData.mediumForce;
        StartAnimation(stateMachine.player.animationData.RunParameterHash);

        startTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.player.animationData.RunParameterHash);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!stateMachine.ReusableData.shouldWalk)
        {
            return;
        }

        if (Time.time < startTime + sprintData.runToWalkTime)
        {
            return;
        }

        StopRunning();
    }

    private void StopRunning()
    {
        if (stateMachine.ReusableData.movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.idlingState);
            return;
        }

        stateMachine.ChangeState(stateMachine.walkingState);
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.mediumStoppingState);

        base.OnMovementCanceled(context);
    }

    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);

        stateMachine.ChangeState(stateMachine.walkingState);
    }


}
