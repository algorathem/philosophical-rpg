using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerAirborneState
{
    private PlayerFallData fallData;
    private Vector3 playerPositionOnEnter;
    public PlayerFallingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
        fallData = airborneData.fallData;
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.player.animationData.FallParameterHash);
        playerPositionOnEnter = stateMachine.player.transform.position;
        stateMachine.ReusableData.movementSpeedMultiplier = 0f;
        ResetVerticalVelocity();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.player.animationData.FallParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        LimitVerticalVelocity();
    }

    protected override void ResetSprintState()
    {
    }

    private void LimitVerticalVelocity()
    {
        Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();
        if (playerVerticalVelocity.y >= -fallData.fallSpeedLimit)
        {
            return;
        }
        Vector3 limitedVelocity = new Vector3(0f, -fallData.fallSpeedLimit - playerVerticalVelocity.y, 0f);
        stateMachine.player.rb.AddForce(limitedVelocity, ForceMode.VelocityChange);
    }

    protected override void OnContactWithGround(Collider collider)
    {
        float fallDistance = playerPositionOnEnter.y - stateMachine.player.transform.position.y;

        if (fallDistance < fallData.minimumDistanceToBeConsideredHardFall)
        {
            stateMachine.ChangeState(stateMachine.lightLandingState);

            return;
        }

        if (stateMachine.ReusableData.shouldWalk && !stateMachine.ReusableData.shouldSprint || stateMachine.ReusableData.movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.hardLandingState);

            return;
        }

        stateMachine.ChangeState(stateMachine.rollingState);

    }
}
