using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementComponent : MonoBehaviour
{
    [SerializeField]
    TrailRenderer trailRenderer;

    [Header("Movement - WalkRun/Running")]
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
    [SerializeField, Range(0, 1f)]
    private float _maxAirAccelerationMultiplier = .5f;

    [SerializeField] 
    private int _maxAirJumps = 1;

    int _currentAirJumps = 0;

    private bool desiredJump = false;

    [Header("Movement - Sliding")] 
    [SerializeField, Range(1, 10f)]
    private float _slideSpeedBoost = 3f;
    
    [SerializeField, Range(0, 1000f)]
    private float _slideAccelerationDecay = 1f;

    [SerializeField] 
    private float _slideCooldownSeconds = 3f;
    float lastSlide = 0f;


    [Header("Movement - Grounded Cast")] 
    [SerializeField, Range(0, 2f)]
    private float _groundedCastMaxDistance = 0.3f;
    [SerializeField, Range(0, 2f)]
    private float _groundedCastRadius = 0.1f;

    [SerializeField]
    private Transform _playerInputSpace = null;

    private Rigidbody _rigidbody;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _desiredVelocity;

    private Vector3 _feetOffset = -Vector3.one;
    private bool _isGrounded;

    public delegate void OnMovementSpeedChange(float speed);

    public OnMovementSpeedChange MoveSpeedChangeEvent = null;

    public delegate void OnJump();

    public OnJump JumpEvent = null;
    public delegate void IsSliding(bool isSliding);

    public IsSliding SlidingEvent = null;
    
    public delegate void IsFalling(bool isFalling);

    public IsFalling FallingEvent = null;
    private bool _isSliding = false;

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

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerInputSpace = Camera.main.transform;
        //trailRenderer = GetComponentInChildren<LineRenderer>();
    }

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

        _isSliding = false;
        return false;
    }

    public void OnMoveInput(Vector2 moveInput)
    {
        MoveInput = moveInput;
    }

    public void OnJumpInput()
    {
        if (_isGrounded || _currentAirJumps < _maxAirJumps) desiredJump = true;
    }

    public void OnSlideInput()
    {
        if (!_isGrounded || _isSliding) return;
        if (Time.time < lastSlide + _slideCooldownSeconds) return;
        if(_velocity.magnitude < _maxSpeed *.1f) return;
        Debug.Log("Currently Sliding");
        _rigidbody.AddForce(transform.forward*_slideSpeedBoost *_maxSpeed, ForceMode.VelocityChange);
        _isSliding = true;
        lastSlide = Time.time;
        _velocity = _rigidbody.velocity;

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

        if (!_isGrounded)
        {
            _currentAirJumps++;
        }

        JumpEvent?.Invoke();
        desiredJump = false;
        _isSliding = false;


    }

    void UpdateState()
    {
        _isGrounded = IsGrounded();
        _velocity = _rigidbody.velocity;
        if (_isSliding && _velocity.magnitude < _maxSpeed*.9) _isSliding = false;
        FallingEvent?.Invoke(!_isGrounded);
        SlidingEvent?.Invoke(_isSliding);
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
        if (_isSliding) _desiredVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        CalculatePlayerSpeedRelativeToCamera();
    }

    void FixedUpdate()
    {
        UpdateState();
        float acceleration;
        if (_isGrounded)
        {

            if (_isSliding)
            {
                acceleration = _slideAccelerationDecay;
            }
            else
            {
                acceleration = _maxGroundedAcceleration;
            }
        }
        else
        {
            acceleration = _maxGroundedAcceleration*_maxAirAccelerationMultiplier;
        }

        float maxSpeedChange = acceleration * Time.deltaTime;
        float horizontalAccelerationX = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, maxSpeedChange);
        float horizontalAccelerationZ = Mathf.MoveTowards(_velocity.z, _desiredVelocity.z, maxSpeedChange);

        _velocity = new Vector3(horizontalAccelerationX, _velocity.y, horizontalAccelerationZ);
        CheckJump();

        _rigidbody.velocity = _velocity;
        if (_velocity.magnitude > 0.1f)
        {
            transform.forward = new Vector3(_velocity.x, 0, _velocity.z);
        }

        float horizontalSpeed = new Vector3(_velocity.x, 0, _velocity.z).magnitude;

        MoveSpeedChangeEvent?.Invoke(horizontalSpeed/_maxSpeed);


        if (_isSliding) trailRenderer.startColor = Color.red;
        else trailRenderer.startColor = Color.blue;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(FeetPosition + new Vector3(0f, -_groundedCastMaxDistance, 0f), _groundedCastRadius);
        Gizmos.DrawLine(FeetPosition, FeetPosition + new Vector3(0f, -_groundedCastMaxDistance, 0f));
    }
}
