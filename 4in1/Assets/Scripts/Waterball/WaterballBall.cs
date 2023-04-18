using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class WaterballBall : NetworkBehaviour {


    public float dragCoefficient = 0;
    public float rollingResistanceCoefficient = 0;
    public float spinResistanceCoefficient = 0; 
    

    private Rigidbody rb;
    
    
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    // [Command]
    public void ApplyForce(Vector3 impactForce, Vector3 impactPosition)
    {
        rb.AddForceAtPosition(impactForce, impactPosition, ForceMode.Impulse);
        // position = rb.position;
        // velocity = rb.velocity;
    }

    private void FixedUpdate() {
        Vector3 velocity = rb.velocity;
        Vector3 angularVelocity = rb.angularVelocity;
        float speed = velocity.magnitude;
        float angularSpeed = angularVelocity.magnitude; 

        float dragMagnitude = speed * speed * dragCoefficient;
        float rollingResistanceMagnitude = rollingResistanceCoefficient;
        float spinResistanceMagnitude = angularSpeed * spinResistanceCoefficient;
        
        Vector3 dragForce = dragMagnitude * -velocity.normalized; 
        Vector3 rollingResistanceForce = rollingResistanceMagnitude * -velocity.normalized; 
        Vector3 spinResistanceTorque = spinResistanceMagnitude * -angularVelocity.normalized; 
        
        rb.AddForce(dragForce + rollingResistanceForce);
        rb.AddTorque(spinResistanceTorque);
    }
}


    
// [SyncVar(hook = nameof(OnPositionChanged))]
// private Vector3 position;

// [SyncVar(hook = nameof(OnVelocityChanged))]
// private Vector3 velocity;




// private void OnPositionChanged(Vector3 oldPosition, Vector3 newPosition)
// {
// rb.position = newPosition;
// }

// private void OnVelocityChanged(Vector3 oldVelocity, Vector3 newVelocity)
// {
// rb.velocity = newVelocity;
// }


