using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHardStoppingState : PlayerStoppingState
{
    public PlayerHardStoppingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.ReusableData.movementDecelerationForce = movementData.stopData.hardDecelerationForce;
        stateMachine.ReusableData.currentJumpForce = airborneData.jumpData.strongForce;
        StartAnimation(stateMachine.player.animationData.HardStopParameterHash);

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.player.animationData.HardStopParameterHash);
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
