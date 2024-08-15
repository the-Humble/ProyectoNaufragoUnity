using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player States", menuName = "ScriptableObjects/Player States/Grounded State", order = 1)]
public class GroundedPlayerState : MovementPlayerState
{
    public override void OnUpdateState()
    {
        base.OnUpdateState();
    }

    public override void OnFixedUpdateState()
    {
        base.OnFixedUpdateState();
        CalculateHorizontalAcceleration();
        FaceDirectionOfMovement();
    }

}
