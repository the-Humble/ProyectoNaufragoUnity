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
    public void OnUpdateState(StateMachine stateMachine);
    public void ProcessDecisions(StateMachine stateMachine)
    {
        foreach (var transition in Transitions)
        {
            if (transition.Key.Decide())
            {
                stateMachine.SwitchState(transition.Value);
                return;
            }
        }
    }
    public void OnFixedUpdateState(StateMachine stateMachine);
    public void OnExitState(StateMachine stateMachine);



}
