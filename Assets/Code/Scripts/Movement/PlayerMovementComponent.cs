using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour
{
    [Header("Movement - Walking/Running")]
    [SerializeField, Range(0, 100f)]
    private float _maxSpeed = 10.0f;
    [SerializeField, Range(0, 1000f)]
    private float _maxGroundedAcceleration = 100f;
    [SerializeField, Range(0, 90f)] 
    private float _maxGroundedAngle = 70f;

    private float _maxGroundedAngleRads => Mathf.Deg2Rad* _maxGroundedAngle;

    [Header("Movement - Jumping")]
    [SerializeField, Range(0, 10f)]
    private float _maxJumpHeight = 5f;
    [SerializeField, Range(0, 10f)]
    private float _maxAirAcceleration = 1f;

    [SerializeField] 
    private int _maxAirJumps = 1;

    int _currentAirJumps = 0;

    private bool desiredJump = false;

    [Header("Movement - Grounded")] 
    [SerializeField, Range(0, 2f)]
    private float _groundedCastMaxDistance = 0.3f;
    [SerializeField, Range(0, 2f)]
    private float _groundedCastRadius = 0.1f;


    private Rigidbody _rigidbody;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _desiredVelocity;

    private Vector3 _feetOffset = -Vector3.one;
    private bool _isGrounded;

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

    private Vector3 FeetPosition => transform.position + FeetOffset;


    public Vector2 MoveInput { get; private set; }

    private Vector3 CalculateFeetOffset()
    {
        if (TryGetComponent<Collider>(out Collider collider))
        {
            Vector3 minCenterPoint = collider.bounds.center;
            minCenterPoint.y = collider.bounds.min.y + _groundedCastRadius * 1.01f;
            return transform.InverseTransformPoint(minCenterPoint);
        }
        return Vector3.zero;
    }
    private bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.SphereCast(FeetPosition, _groundedCastRadius, Vector3.down, out hit, _groundedCastMaxDistance))
        {
            if (hit.normal.y >= Mathf.Cos(_maxGroundedAngleRads))
            {
                _currentAirJumps = 0;
                return true;
            }
        }
        return false;
    }

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void OnMoveInput(Vector2 moveInput)
    {
        MoveInput = moveInput;
    }

    public void OnJumpInput()
    {
        if (_isGrounded || _currentAirJumps < _maxAirJumps) desiredJump = true;
    }


    private void CheckJump()
    {
        if (!desiredJump) return;

        float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * _maxJumpHeight);

        if (_velocity.y > 0f)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - _velocity.y, 0f);
        }

        _velocity.y += jumpSpeed; 
        _rigidbody.velocity = _velocity;

        if (!_isGrounded) _currentAirJumps++;
        desiredJump = false;
    }

    void UpdateState()
    {
        _isGrounded = IsGrounded();
        _velocity = _rigidbody.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        _desiredVelocity = new Vector3(MoveInput.x, 0f, MoveInput.y) * _maxSpeed;
    }

    void FixedUpdate()
    {
        UpdateState();
        float acceleration = _isGrounded ? _maxGroundedAcceleration : _maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;
        float horizontalAccelerationX = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, maxSpeedChange);
        float horizontalAccelerationZ = Mathf.MoveTowards(_velocity.z, _desiredVelocity.z, maxSpeedChange);

        _velocity = new Vector3(horizontalAccelerationX, _velocity.y, horizontalAccelerationZ);
        CheckJump();

        _rigidbody.velocity = _velocity;

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(FeetPosition + new Vector3(0f, -_groundedCastMaxDistance, 0f), _groundedCastRadius);
        Gizmos.DrawLine(FeetPosition, FeetPosition + new Vector3(0f, -_groundedCastMaxDistance, 0f));
    }
}
