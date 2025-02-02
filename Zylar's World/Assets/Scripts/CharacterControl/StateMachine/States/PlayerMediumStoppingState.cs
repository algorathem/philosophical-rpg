using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMediumStoppingState : PlayerStoppingState
{
    public PlayerMediumStoppingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.ReusableData.movementDecelerationForce = movementData.stopData.mediumDecelerationForce;
        stateMachine.ReusableData.currentJumpForce = airborneData.jumpData.mediumForce;
        StartAnimation(stateMachine.player.animationData.MediumStopParameterHash);

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.player.animationData.MediumStopParameterHash);
    }
}
