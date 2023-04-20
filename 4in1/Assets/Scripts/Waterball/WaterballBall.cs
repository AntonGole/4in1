using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class WaterballBall : NetworkBehaviour {


    public float dragCoefficient = 0;
    public float rollingResistanceCoefficient = 0;
    public float spinResistanceCoefficient = 0;

    public float spawningDuration = 1.5f;

    private Rigidbody rb;
    private float tStart;

    public LayerMask thudsOn;

    private void Start() {
        tStart = Time.time;
        rb = GetComponent<Rigidbody>();
    }


    // [Command]
    public void ApplyForce(Vector3 impactForce, Vector3 impactPosition) {
        rb.AddForceAtPosition(impactForce, impactPosition, ForceMode.Impulse);
        // position = rb.position;
        // velocity = rb.velocity;
    }

    private void FixedUpdate() {
        if (Time.time - tStart < spawningDuration) {
            return;
        }

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

        rb.AddTorque(spinResistanceTorque);
        rb.AddForce(dragForce + rollingResistanceForce);
    }


    private void OnCollisionEnter(Collision collision) {
        if (thudsOn == (thudsOn | (1 << collision.gameObject.layer))) {
            
            
            
            
            var audioManager = WaterballAudioManager.Instance;
            audioManager.PlaySoundEffect(audioManager.thud, rb.velocity.magnitude/15f);
        }
    }

// var  = collision.gameObject; 

    // if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Floor") )
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


