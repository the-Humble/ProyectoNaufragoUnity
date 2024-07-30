using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public IState<StateMachine> currentState = null;

    [SerializeField] private IState<StateMachine> initialState;

    public void Awake()
    {
        if (initialState!=null)
        {
            SwitchState(initialState);
        }
    }

    public void SwitchState(IState<StateMachine> newState)
    {
        if(currentState!=null)
            currentState.OnExitState(this);

        currentState = newState;
        currentState.OnEnterState(this);
    }


}
