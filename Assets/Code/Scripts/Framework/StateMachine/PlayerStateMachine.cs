using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Input;
using UnityEditor.Recorder.Input;
using UnityEngine;

public class PlayerStateMachine : InputBasedStateMachine<PlayerInputActions>
{
    public PlayerMovementComponent PlayerMovementComponent { get; private set; }
    public PlayerAnimationController PlayerAnimationController { get; private set; }
    public CinemachineFreeLook PlayerCamera => _playerCamera;

    [SerializeField] private PlayerStats _playerBaseStats;
    public PlayerStats PlayerBaseStats => _playerBaseStats;

    [SerializeField] private CinemachineFreeLook _playerCamera;
    [SerializeField] private PlayerBaseState _initialPlayerState;


    public override void Awake()
    {
        initialState = _initialPlayerState;
        base.Awake();
        SetupInputController();
        PlayerMovementComponent = GetComponent<PlayerMovementComponent>();
        PlayerAnimationController = GetComponent<PlayerAnimationController>();
        _playerCamera = GetComponentInChildren<CinemachineFreeLook>();

    }

    public override void SetupInputController()
    {
        inputActionCollection = new PlayerInputActions();
    }
}
