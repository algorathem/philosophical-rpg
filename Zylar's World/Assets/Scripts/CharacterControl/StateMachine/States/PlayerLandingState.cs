using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLandingState : PlayerGroundedState
{
    public PlayerLandingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.player.animationData.LandingParameterHash);

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.player.animationData.LandingParameterHash);
    }

}
