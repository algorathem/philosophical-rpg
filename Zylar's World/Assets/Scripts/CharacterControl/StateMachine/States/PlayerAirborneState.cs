using UnityEngine;

public class PlayerAirborneState : PlayerMovementState
{
    public PlayerAirborneState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.player.animationData.AirborneParameterHash);
        ResetSprintState();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.player.animationData.AirborneParameterHash);
    }

    protected override void OnContactWithGround(Collider collider)
    {
        stateMachine.ChangeState(stateMachine.lightLandingState);
    }

    protected virtual void ResetSprintState()
    {
        stateMachine.ReusableData.shouldSprint = false;
    }

}
