using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlayerState : MovementPlayerState
{
    public override void OnEnterState(StateMachine stateMachine)
    {
        base.OnEnterState(stateMachine);
        _maxSpeed = StateMachineController.PlayerBaseStats.Falling.MaxAirSpeed;
        _maxAcceleration = StateMachineController.PlayerBaseStats.Falling.AirAccelerationMultiplier * StateMachineController.PlayerBaseStats.Grounded.WalkRun.MaxGroundAcceleration;
        StateMachineController.PlayerAnimationController.PlayerAnimator.Play("Falling");
    }
}
