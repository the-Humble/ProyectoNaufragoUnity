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

    protected Vector3 FeetPosition => StateMachine.transform.position + FeetOffset;
    public override void InitState(PlayerStateMachine stateMachine, ref PlayerInputActions playerInputActions)
    {
        base.InitState(stateMachine, ref playerInputActions);
        _maxSpeed = stateMachine.PlayerBaseStats.Grounded.WalkRun.MaxGroundSpeed;
    }

    public override void OnEnterState(PlayerStateMachine stateMachine)
    {
        base.OnEnterState(stateMachine);
        _playerInputSpace = Camera.main.transform;
        _rigidbody = stateMachine.GetComponentInChildren<Rigidbody>();
    }

    public override void OnUpdateState(PlayerStateMachine stateMachine)
    {
        base.OnUpdateState(stateMachine);
        CalculatePlayerSpeedRelativeToCamera();
    }

    public override void OnFixedUpdateState(PlayerStateMachine stateMachine)
    {
        base.OnFixedUpdateState(stateMachine);
        _velocity = _rigidbody.velocity;
    }
    public override void SetupInputListeners(ref PlayerInputActions playerInputActions)
    {
        playerInputActions.General.Move.performed += OnMovePerformed;
    }

    public override void RemoveInputListeners(ref PlayerInputActions playerInputActions)
    {
        playerInputActions.General.Move.performed -= OnMovePerformed;
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
    }

    private Vector3 CalculateFeetOffset()
    {
        if (StateMachine.TryGetComponent<Collider>(out Collider collider))
        {
            Vector3 minCenterPoint = collider.bounds.center;
            minCenterPoint.y = collider.bounds.min.y + _groundedCastRadius * 1.01f;
            return StateMachine.transform.InverseTransformPoint(minCenterPoint);
        }
        return Vector3.zero;
    }

    protected void CalculateHorizontalAcceleration()
    {
        float maxSpeedChange = _maxAcceleration * Time.deltaTime;
        float horizontalAccelerationX = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, maxSpeedChange);
        float horizontalAccelerationZ = Mathf.MoveTowards(_velocity.z, _desiredVelocity.z, maxSpeedChange);

        _velocity = new Vector3(horizontalAccelerationX, _velocity.y, horizontalAccelerationZ);
    }

    protected void FaceDirectionOfMovement()
    {
        _rigidbody.velocity = _velocity;
        if (_velocity.magnitude > 0.1f)
        {
            StateMachine.transform.forward = new Vector3(_velocity.x, 0, _velocity.z);
        }
    }
}