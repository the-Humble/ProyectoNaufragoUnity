using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        currentState.OnUpdateState(this);
    }

    public void FixedUpdate()
    {
        currentState.OnUpdateState(this);
    }

    public void SwitchState(IState newState)
    {
        Debug.Log("[State Machine] State Machine transitioning to state: " + newState.Name);

        if(currentState!=null)
            currentState.OnExitState(this);

        currentState = newState;
        currentState.OnEnterState(this);
    }
}
