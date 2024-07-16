using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionCallbacks : MonoBehaviour
{
    public delegate void OnCollisionEnterCallback(Collision collision);
    public delegate void OnCollisionExitCallback(Collision collision);
    public delegate void OnCollisionStayCallback(Collision collision);
    
    public delegate void OnTriggerEnterCallback(Collider other);
    public delegate void OnTriggerExitCallback(Collider other);
    public delegate void OnTriggerStayCallback(Collider other);

    public OnCollisionEnterCallback OnCollisionEnterEvent = null;
    public OnCollisionExitCallback OnCollisionExitEvent = null;
    public OnCollisionStayCallback OnCollisionStayEvent = null;

    public OnTriggerEnterCallback OnTriggerEnterEvent = null;
    public OnTriggerExitCallback OnTriggerExitEvent = null;
    public OnTriggerStayCallback OnTriggerStayEvent = null;

    void OnCollisionEnter(Collision collision)
    {
        OnCollisionEnterEvent?.Invoke(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        OnCollisionExitEvent?.Invoke(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        OnCollisionStayEvent?.Invoke(collision);
    }

    void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }

    void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent?.Invoke(other);
    }

    void OnTriggerStay(Collider other)
    {
        OnTriggerStayEvent?.Invoke(other);
    }
}
