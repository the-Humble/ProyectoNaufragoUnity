using System;
using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;

public abstract class PlayerBaseState : InputListenerState<PlayerStateMachine, PlayerInputActions>
{
    public override void OnEnterState(StateMachine stateMachine)
    {
        if (stateMachine is PlayerStateMachine)
        {
            PlayerStateMachine playerStateMachine = (PlayerStateMachine)stateMachine;
            InitState(playerStateMachine, PlayerStateMachine.PlayerInputActions);
            base.OnEnterState(stateMachine);
        }
    }
}
