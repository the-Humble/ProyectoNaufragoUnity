using System;
using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;

public abstract class PlayerBaseState : InputListenerState<PlayerControllerStateMachine, PlayerInputActions>
{
    public override void OnEnterState(StateMachine stateMachine)
    {
        if (stateMachine is PlayerControllerStateMachine)
        {
            PlayerControllerStateMachine playerControllerStateMachine = (PlayerControllerStateMachine)stateMachine;
            InitInputState(playerControllerStateMachine, PlayerControllerStateMachine.PlayerInputActions);
            base.OnEnterState(stateMachine);
        }
    }
}
