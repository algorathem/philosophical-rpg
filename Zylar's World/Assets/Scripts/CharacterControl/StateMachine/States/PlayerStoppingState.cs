using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStoppingState : PlayerGroundedState
{
    public PlayerStoppingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ReusableData.movementSpeedMultiplier = 0f;
        SetBaseCameraRecenteringData();

        base.Enter();
        StartAnimation(stateMachine.player.animationData.StoppingParameterHash);

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.player.animationData.StoppingParameterHash);

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        RotateTowardsTargetRotation();

        if (!IsMovingHorizontally())
        {
            return;
        }
        DecelerateHorizontally();
    }

    protected override void AddInputActionsCallback()
    {
        base.AddInputActionsCallback();
        stateMachine.player.playerInput.PlayerActions.Movement.started += OnMovementStarted;
    }

    protected override void RemoveInputActionsCallback()
    {
        base.RemoveInputActionsCallback();
        stateMachine.player.playerInput.PlayerActions.Movement.started -= OnMovementStarted;
    }

    public override void OnAnimationTransitionEvent()
    {
        stateMachine.ChangeState(stateMachine.idlingState);
    }

    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        OnMove();
    }

}
