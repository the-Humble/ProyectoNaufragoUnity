using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovementComponent PlayerMovementComponent { get; private set; }
    
    [SerializeField] private CinemachineFreeLook _playerCamera;
    void Awake()
    {
        PlayerMovementComponent = GetComponent<PlayerMovementComponent>();
        _playerCamera = GetComponentInChildren<CinemachineFreeLook>();
    }
}
