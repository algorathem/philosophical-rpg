using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdlingState : PlayerGroundedState
{
    private PlayerIdleData idleData;
    public PlayerIdlingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
        idleData = movementData.idleData;
    }

    public override void Enter()
    {
        stateMachine.ReusableData.movementSpeedMultiplier = 0f;
        stateMachine.ReusableData.backwardsCameraRecenteringData = idleData.backwardsCameraRecenteringData;

        base.Enter();

        StartAnimation(stateMachine.player.animationData.IdleParameterHash);

        stateMachine.ReusableData.currentJumpForce = airborneData.jumpData.stationaryForce;
        ResetVelocity();
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.player.animationData.IdleParameterHash);

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (stateMachine.ReusableData.movementInput == Vector2.zero)
        {
            return;
        }

        OnMove();
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

}
