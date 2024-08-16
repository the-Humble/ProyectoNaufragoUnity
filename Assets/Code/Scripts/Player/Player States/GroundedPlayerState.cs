using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player States", menuName = "ScriptableObjects/Player States/Grounded State", order = 1)]
public class GroundedPlayerState : MovementPlayerState
{
    public override void OnEnterState(StateMachine stateMachine)
    {
        base.OnEnterState(stateMachine);
        _maxSpeed = StateMachineController.PlayerBaseStats.Grounded.WalkRun.MaxGroundSpeed;
        _maxAcceleration = StateMachineController.PlayerBaseStats.Grounded.WalkRun.MaxGroundAcceleration;
        StateMachineController.PlayerAnimationController.PlayerAnimator.Play("Locomotion");
    }

    public override void OnUpdateState()
    {
        base.OnUpdateState();
        StateMachineController.PlayerAnimationController.SetMovementSpeed(_velocity.magnitude/_maxSpeed);
    }

    public override void OnFixedUpdateState()
    {
        base.OnFixedUpdateState();
        CalculateHorizontalAcceleration();
        FaceDirectionOfMovement();
    }

}
