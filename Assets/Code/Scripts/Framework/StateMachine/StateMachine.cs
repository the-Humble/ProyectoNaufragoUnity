using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachine : MonoBehaviour
{

    protected IState currentState;
    protected IState initialState;

    public virtual void Awake()
    {
        if (initialState!=null)
        {
            SwitchState(initialState);
        }
    }

    public void Update()
    {
        currentState.OnUpdateState();
    }

    public void FixedUpdate()
    {
        currentState.OnFixedUpdateState();
    }

    public void SwitchState(IState newState)
    {
        Debug.Log("[State Machine] State Machine transitioning to state: " + newState.Name);

        if(currentState!=null)
            currentState.OnExitState();

        currentState = newState;
        currentState.OnEnterState(this);
    }
}
