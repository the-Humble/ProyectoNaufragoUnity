using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Grounded
{
    [SerializeField, Range(0, 90f)]
    private float _maxGroundedAngle;
    [SerializeField, Range(0, 2f)]
    private float _groundedCastMaxDistance;
    [SerializeField, Range(0, 2f)]
    private float _groundedCastRadius;

    public WalkRun WalkRun;
    public Sliding Sliding;

    public float MaxGroundedAngle => _maxGroundedAngle;
    public float MaxGroundedAngleRads => Mathf.Deg2Rad * _maxGroundedAngle;
    public float GroundedCatMaxDistance => _groundedCastMaxDistance;
    public float GroundedCastRadius => _groundedCastRadius;
}

[Serializable]
public struct WalkRun
{
    [Header("Grounded - WalkRun")] 
    [SerializeField]
    private float _maxGroundSpeed;
    [SerializeField]
    private float _maxGroundAcceleration;
    public float MaxGroundSpeed => _maxGroundSpeed;
    public float MaxGroundAcceleration => _maxGroundAcceleration;
}
[Serializable]
public struct Sliding
{
    [Header("Grounded - Sliding")]
    [SerializeField]
    private float _slideDecay;

    public float SlideDecay => _slideDecay;
}
[CreateAssetMenu(fileName = "Player Stats", menuName = "ScriptableObjects/Player Stats", order = 1)]
public class PlayerStats : ScriptableObject
{

    [SerializeField]
    private Grounded _grounded = new();
    public Grounded Grounded
    {
        get => _grounded;
    }

}
