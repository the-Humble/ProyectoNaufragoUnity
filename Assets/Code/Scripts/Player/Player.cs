using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementComponent))]
[RequireComponent(typeof(PlayerAnimationController))]
public class Player : MonoBehaviour
{
    public PlayerMovementComponent PlayerMovementComponent { get; private set; }
    public PlayerAnimationController PlayerAnimationController { get; private set; }
    
    [SerializeField] private CinemachineFreeLook _playerCamera;
    void Awake()
    {
        PlayerMovementComponent = GetComponent<PlayerMovementComponent>();
        PlayerAnimationController = GetComponent<PlayerAnimationController>();
        _playerCamera = GetComponentInChildren<CinemachineFreeLook>();
    }

    void OnEnable()
    {
        PlayerMovementComponent.MoveSpeedChangeEvent+=PlayerAnimationController.SetMovementSpeed;
        PlayerMovementComponent.FallingEvent+=PlayerAnimationController.SetIsFalling;
        PlayerMovementComponent.SlidingEvent+=PlayerAnimationController.SetIsSliding;
        PlayerMovementComponent.JumpEvent+=PlayerAnimationController.TriggerJump;
    }
    void OnDisable()
    {
        PlayerMovementComponent.MoveSpeedChangeEvent-=PlayerAnimationController.SetMovementSpeed;
        PlayerMovementComponent.FallingEvent-=PlayerAnimationController.SetIsFalling;
        PlayerMovementComponent.SlidingEvent-=PlayerAnimationController.SetIsSliding;
        PlayerMovementComponent.JumpEvent-=PlayerAnimationController.TriggerJump;
    }
}
