using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputListener<T> where T : IInputActionCollection
{
    public void SetupInputListeners(T playerInputAction);
    public void RemoveInputListeners(T playerInputAction);
}
