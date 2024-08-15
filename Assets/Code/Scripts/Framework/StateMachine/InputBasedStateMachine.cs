using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputBasedStateMachine<T> : StateMachine, IInputProcessor where T : IInputActionCollection
{
    protected T inputActionCollection;
    public abstract void SetupInputController();
}
