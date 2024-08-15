using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputListener<T> where T:IInputActionCollection
{
    public void SetupInputListeners(ref T inputActionCollection);
    public void RemoveInputListeners(ref T inputActionCollection);
}
