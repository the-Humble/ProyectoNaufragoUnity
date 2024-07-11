using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace Input
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField]
        private Player _player;
        [SerializeField]
        private PlayerInputActions _playerInputActions;

        public void Awake()
        {
            _player = GetComponentInParent<Player>();
            _playerInputActions = new PlayerInputActions();

        }

        public void OnEnable()
        {
            _playerInputActions.Enable();
            _playerInputActions.General.Move.performed += OnMovePerformed;
            _playerInputActions.General.Move.canceled += OnMovePerformed;
            _playerInputActions.General.Jump.performed += OnJumpPerformed;
        }


        public void OnDisable()
        {
            _playerInputActions.Disable();
            _playerInputActions.General.Move.performed -= OnMovePerformed;
            _playerInputActions.General.Move.canceled -= OnMovePerformed;
            _playerInputActions.General.Jump.performed -= OnJumpPerformed;

        }
        private void OnJumpPerformed(InputAction.CallbackContext obj)
        {
            Debug.Log("Performing Jump");
            _player.PlayerMovementComponent.OnJumpInput();
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            Debug.Log("Performing Move");
            Vector2 moveInput = context.ReadValue<Vector2>().normalized;
            _player.PlayerMovementComponent.OnMoveInput(moveInput);
        }
    }
}
