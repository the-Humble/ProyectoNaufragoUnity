using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public interface IState
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Dictionary<IDecision, IState> Transitions { get; set; }
    public void OnEnterState(StateMachine stateMachine);
    public void OnUpdateState();
    public void OnFixedUpdateState();
    public void OnExitState();



}
