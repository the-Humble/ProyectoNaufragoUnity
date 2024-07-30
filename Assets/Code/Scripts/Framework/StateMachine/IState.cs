using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T> where T : StateMachine
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Dictionary<IDecision, IState<T>> Transitions { get; set; }
    public void OnEnterState(T stateMachine);
    public void OnUpdateState(T stateMachine);
    public void OnFixedUpdateState(T stateMachine);
    public void OnExitState(T stateMachine);



}
