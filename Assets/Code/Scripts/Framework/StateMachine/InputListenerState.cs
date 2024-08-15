using Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputListenerState<T, W>
    : ScriptableObject, IInputListener<W>, IState
    where T : StateMachine
    where W : IInputActionCollection

{
    protected T StateMachine;
    protected W InputActionCollection;

    [SerializeField]
    private string name = "New State";
    [SerializeField]
    private string description = "New State Description";
    public string Name
    {
        get => name;
        set => name = value;
    }

    public string Description 
    {
        get=>description;
        set=> description = value;
    }

    public Dictionary<IDecision, IState> Transitions { get; set; }

    public virtual void InitState(T stateMachine, ref W playerInputActions)
    {
        InputActionCollection = playerInputActions;
        StateMachine = stateMachine;
    }

    public virtual void OnEnterState(StateMachine stateMachine)
    {
        SetupInputListeners(ref InputActionCollection);
    }

    public virtual void OnUpdateState(StateMachine stateMachine)
    {}

    public virtual void OnFixedUpdateState(StateMachine stateMachine)
    {}

    public virtual void OnExitState(StateMachine stateMachine)
    {
        RemoveInputListeners(ref InputActionCollection);
    }

    public abstract void SetupInputListeners(ref W inputActionCollection);

    public abstract void RemoveInputListeners(ref W inputActionCollection);
}
