using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputProcessor<T> where T : IInputActionCollection
{
    public void SetupInputListeners(ref T playerInputActions);
    public void RemoveInputListeners(ref T playerInputActions);
}
