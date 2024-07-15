using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator _animationController;
    void Awake()
    {
        _animationController = GetComponentInChildren<Animator>();
    }

    public void SetMovementSpeed(float movementSpeed)
    {
        _animationController.SetFloat("MovementSpeed", movementSpeed);
    }

    public void SetIsFalling(bool isFalling)
    {
        _animationController.SetBool("Falling", isFalling);
    }
    public void SetIsSliding(bool isSliding)
    {
        _animationController.SetBool("Sliding", isSliding);
    }

    public void TriggerJump()
    {
        _animationController.SetTrigger("Jump");
    }
}
