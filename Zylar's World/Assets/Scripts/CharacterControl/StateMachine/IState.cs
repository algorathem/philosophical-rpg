using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Enter();
    public void Exit();
    public void HandleInput();
    public void LogicUpdate();
    public void PhysicsUpdate();
    public void UiUpdate();
    public void OnAnimationEnterEvent();
    public void OnAnimationExitEvent();
    public void OnAnimationTransitionEvent();
    public void OnTriggerEnter(Collider collider);
    public void OnTriggerExit(Collider collider);
}
