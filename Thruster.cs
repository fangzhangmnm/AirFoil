using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    [Range(0,1)]public float value=0;
    public float maxThrust=1;
    Rigidbody body;
    private void Start()
    {
        body = GetComponentInParent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        body.AddForceAtPosition(transform.forward * value * maxThrust, transform.position);
    }
}
