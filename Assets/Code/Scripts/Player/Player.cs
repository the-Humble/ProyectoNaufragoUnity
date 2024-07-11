using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovementComponent PlayerMovementComponent { get; private set; }

    void Awake()
    {
        PlayerMovementComponent = GetComponent<PlayerMovementComponent>();
    }
}
