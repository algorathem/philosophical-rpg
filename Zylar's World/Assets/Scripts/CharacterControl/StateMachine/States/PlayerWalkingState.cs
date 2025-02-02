using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkingState : PlayerMovingState
{
    private PlayerWalkData walkData;
    public PlayerWalkingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
        walkData = movementData.walkData;
    }

    public override void Enter()
    {
        stateMachine.ReusableData.movementSpeedMultiplier = movementData.walkData.speedMultiplier;
        stateMachine.ReusableData.backwardsCameraRecenteringData = walkData.backwardsCameraRecenteringData;

        base.Enter();
        stateMachine.ReusableData.currentJumpForce = airborneData.jumpData.weakForce;
        StartAnimation(stateMachine.player.animationData.WalkParameterHash);

    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.player.animationData.WalkParameterHash);
        SetBaseCameraRecenteringData();
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.lightStoppingState);

        base.OnMovementCanceled(context);
    }

    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);

        stateMachine.ChangeState(stateMachine.runningState);
    }
}
