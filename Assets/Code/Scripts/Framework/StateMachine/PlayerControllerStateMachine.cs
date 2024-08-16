using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Input;
using UnityEditor.Recorder.Input;
using UnityEngine;

public class PlayerControllerStateMachine : InputBasedStateMachine
{
    public PlayerMovementComponent PlayerMovementComponent { get; private set; }
    public PlayerAnimationController PlayerAnimationController { get; private set; }
    public CinemachineFreeLook PlayerCamera => _playerCamera;

    [SerializeField] private PlayerStats _playerBaseStats;
    public PlayerStats PlayerBaseStats => _playerBaseStats;

    [SerializeField] private CinemachineFreeLook _playerCamera;
    [SerializeField] private PlayerBaseState _initialPlayerState;

    public static PlayerInputActions PlayerInputActions;


    public override void Awake()
    {
        SetupInputController();
        initialState = _initialPlayerState;
        PlayerMovementComponent = GetComponent<PlayerMovementComponent>();
        PlayerAnimationController = GetComponent<PlayerAnimationController>();
        _playerCamera = GetComponentInChildren<CinemachineFreeLook>();
        base.Awake();

    }

    public override void SetupInputController()
    {
        PlayerInputActions = new PlayerInputActions();
        PlayerInputActions.Enable();
    }
}
