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
    protected T StateMachineController;
    protected W PlayerInputActions;

    [SerializeField]
    private string _name = "New State";
    [SerializeField]
    private string _description = "New State Description";
    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public string Description 
    {
        get=>_description;
        set=> _description = value;
    }

    public Dictionary<IDecision, IState> Transitions { get; set; }

    public virtual void InitInputState(T stateMachine, W playerInputActions)
    {
        Debug.Log("[Input Listener State] Initializing State: " + Name);

        StateMachineController = stateMachine;
        PlayerInputActions = playerInputActions;
    }

    public virtual void OnEnterState(StateMachine stateMachine)
    {
        Debug.Log("[Input Listener State] Entering State: " + Name);
        SetupInputListeners(PlayerInputActions);
    }

    public virtual void OnUpdateState()
    {}

    public virtual void OnFixedUpdateState()
    {}

    public virtual void OnExitState()
    {
        RemoveInputListeners(PlayerInputActions);
    }

    public abstract void SetupInputListeners(W playerInputActions);

    public abstract void RemoveInputListeners(W playerInputActions);
}
