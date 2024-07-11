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
    private float _maxAcceleration = 100f;

    [Header("Movement - Jumping")]
    [SerializeField, Range(0, 10f)]
    private float _maxJumpHeight = 5f;

    [Header("Movement - Grounded")] 
    [SerializeField, Range(0, 2f)]
    private float _groundedCastMaxDistance = 0.3f;
    [SerializeField, Range(0, 2f)]
    private float _groundedCastRadius = 0.1f;


    private Rigidbody _rigidbody;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _desiredVelocity;

    private Vector3 _feetOffset = -Vector3.one;
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
            return true;
        }
        return false;
    }

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void OnJumpInput()
    {
        TryJump();
    }

    public void OnMoveInput(Vector2 moveInput)
    {
        MoveInput = moveInput;
    }
    private void TryJump()
    {
        if (!IsGrounded()) return;
        _velocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * _maxJumpHeight);
        _rigidbody.velocity = _velocity;
    }

    // Update is called once per frame
    void Update()
    {
        _desiredVelocity = new Vector3(MoveInput.x, 0f, MoveInput.y) * _maxSpeed;
    }

    void FixedUpdate()
    {
        _velocity = _rigidbody.velocity;
        float maxSpeedChange = _maxAcceleration * Time.deltaTime;
        float horizontalAccelerationX = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, maxSpeedChange);
        float horizontalAccelerationZ = Mathf.MoveTowards(_velocity.z, _desiredVelocity.z, maxSpeedChange);

        _velocity = new Vector3(horizontalAccelerationX, _velocity.y, horizontalAccelerationZ);

        _rigidbody.velocity = _velocity;

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(FeetPosition + new Vector3(0f, -_groundedCastMaxDistance, 0f), _groundedCastRadius);
        Gizmos.DrawLine(FeetPosition, FeetPosition + new Vector3(0f, -_groundedCastMaxDistance, 0f));
    }
}
