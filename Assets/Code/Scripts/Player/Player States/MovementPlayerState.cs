using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerMovementComponent;

public abstract class MovementPlayerState : PlayerBaseState
{

    protected Rigidbody _rigidbody;

    protected float _maxSpeed = 10.0f;
    protected float _maxAcceleration = 100f;
    protected float _groundedCastRadius = 0.1f;
    protected bool _isGrounded = false;
    

    protected Transform _playerInputSpace = null;
    public Vector2 MoveInput { get; private set; }

    protected Vector3 _velocity = Vector3.zero;
    protected Vector3 _desiredVelocity;
    protected Vector3 _feetOffset = -Vector3.one;
    private Vector3 FeetOffset
    {
        get
        {
            if (_feetOffset == -Vector3.one)
            {
                _feetOffset = CalculateFeetOffset();
            }
            return _feetOffset;
        }
    }

    protected Vector3 FeetPosition => StateMachineController.transform.position + FeetOffset;
    public override void InitInputState(PlayerControllerStateMachine controllerStateMachine, PlayerInputActions playerInputActions)
    {
        base.InitInputState(controllerStateMachine, playerInputActions);
    }

    public override void OnEnterState(StateMachine stateMachine)
    {
        base.OnEnterState(stateMachine);
        _playerInputSpace = Camera.main.transform;
        _rigidbody = StateMachineController.GetComponentInChildren<Rigidbody>();
    }

    public override void OnUpdateState()
    {
        base.OnUpdateState();
        CalculatePlayerSpeedRelativeToCamera();
    }

    public override void OnFixedUpdateState()
    {
        base.OnFixedUpdateState();
        _velocity = _rigidbody.velocity;
        _isGrounded = IsGrounded();
        CalculateHorizontalAcceleration();
        FaceDirectionOfMovement();
    }
    public override void SetupInputListeners(PlayerInputActions playerInputActions)
    {
        playerInputActions.General.Move.performed += OnMovePerformed;
        playerInputActions.General.Move.canceled += OnMovePerformed;
    }

    public override void RemoveInputListeners(PlayerInputActions playerInputActions)
    {
        playerInputActions.General.Move.performed -= OnMovePerformed;
        playerInputActions.General.Move.canceled -= OnMovePerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Debug.Log("[Movement Player State] Performing Move");
        Vector2 moveInput = context.ReadValue<Vector2>();
        MoveInput = moveInput;
    }

    private void CalculatePlayerSpeedRelativeToCamera()
    {
        Vector3 forward = _playerInputSpace.forward;
        forward.y = 0f;
        forward.Normalize();
        Vector3 right = _playerInputSpace.right;
        right.y = 0f;
        right.Normalize();
        _desiredVelocity = (forward * MoveInput.y + right * MoveInput.x) * _maxSpeed;
        //Debug.Log("[MovementPlayerState] Desired Velocity: " + _desiredVelocity);
    }

    private Vector3 CalculateFeetOffset()
    {
        if (StateMachineController.TryGetComponent<Collider>(out Collider collider))
        {
            Vector3 minCenterPoint = collider.bounds.center;
            minCenterPoint.y = collider.bounds.min.y + _groundedCastRadius * 1.01f;
            return StateMachineController.transform.InverseTransformPoint(minCenterPoint);
        }
        return Vector3.zero;
    }

    protected void CalculateHorizontalAcceleration()
    {
        float maxSpeedChange = _maxAcceleration * Time.deltaTime;
        float horizontalAccelerationX = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, maxSpeedChange);
        float horizontalAccelerationZ = Mathf.MoveTowards(_velocity.z, _desiredVelocity.z, maxSpeedChange);

        _velocity = new Vector3(horizontalAccelerationX, _velocity.y, horizontalAccelerationZ);
        //Debug.Log("[MovementPlayerState] Velocity after Horizontal Acceleration: " + _velocity);
    }

    protected void FaceDirectionOfMovement()
    {
        _rigidbody.velocity = _velocity;
        if (_velocity.magnitude > 0.1f)
        {
            StateMachineController.transform.forward = new Vector3(_velocity.x, 0, _velocity.z);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.SphereCast(FeetPosition, _groundedCastRadius, Vector3.down, out hit, StateMachineController.PlayerBaseStats.Grounded.GroundedCatMaxDistance))
        {
            if (hit.normal.y >= Mathf.Cos(StateMachineController.PlayerBaseStats.Grounded.MaxGroundedAngleRads))
            {
                return true;
            }
        }
        return false;
    }
}