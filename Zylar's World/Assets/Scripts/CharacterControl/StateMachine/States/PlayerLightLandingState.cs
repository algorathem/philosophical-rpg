using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightLandingState : PlayerLandingState
{
    public PlayerLightLandingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ReusableData.movementSpeedMultiplier = 0f;
        base.Enter();
        stateMachine.ReusableData.currentJumpForce = airborneData.jumpData.stationaryForce;
        ResetVelocity();
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

    public override void OnAnimationTransitionEvent()
    {
        stateMachine.ChangeState(stateMachine.idlingState);
    }
}
