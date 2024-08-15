using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputBasedStateMachine : StateMachine, IInputProcessor
{
    public abstract void SetupInputController();
}
