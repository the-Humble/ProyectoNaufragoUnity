using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRootState<T> : IState<T> where T : StateMachine
{
    IState<T> SubState { get; set; }

}
