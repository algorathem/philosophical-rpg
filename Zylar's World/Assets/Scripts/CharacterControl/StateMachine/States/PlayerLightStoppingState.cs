using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightStoppingState : PlayerStoppingState
{
    public PlayerLightStoppingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.ReusableData.movementDecelerationForce = movementData.stopData.lightDecelerationForce;
        stateMachine.ReusableData.currentJumpForce = airborneData.jumpData.weakForce;

    }
}
