using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRollingState : PlayerLandingState
{
    private PlayerRollData rollData;
    public PlayerRollingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
        rollData = movementData.rollData;
    }

    public override void Enter()
    {
        stateMachine.ReusableData.movementSpeedMultiplier = rollData.speedMultiplier;
        base.Enter();
        stateMachine.ReusableData.shouldSprint = false;
        StartAnimation(stateMachine.player.animationData.RollParameterHash);

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.player.animationData.RollParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (stateMachine.ReusableData.movementInput != Vector2.zero)
        {
            return;
        }
        RotateTowardsTargetRotation();
    }

    public override void OnAnimationTransitionEvent()
    {
        if (stateMachine.ReusableData.movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.mediumStoppingState);

            return;
        }

        OnMove();
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
    }

}
