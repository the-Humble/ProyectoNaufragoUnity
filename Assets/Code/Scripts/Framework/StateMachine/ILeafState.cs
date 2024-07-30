using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILeafState<T> : IState<T> where T : StateMachine
{

}
