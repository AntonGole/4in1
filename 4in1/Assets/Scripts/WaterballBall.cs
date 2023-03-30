using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class WaterballBall : NetworkBehaviour
{

    
    [SyncVar(hook = nameof(OnPositionChanged))]
    private Vector3 position;

    [SyncVar(hook = nameof(OnVelocityChanged))]
    private Vector3 velocity;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnPositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {
        rb.position = newPosition;
    }

    private void OnVelocityChanged(Vector3 oldVelocity, Vector3 newVelocity)
    {
        rb.velocity = newVelocity;
    }

    [Command]
    public void CmdApplyForce(Vector3 impactForce, Vector3 impactPosition)
    {
        rb.AddForceAtPosition(impactForce, impactPosition, ForceMode.Impulse);
        position = rb.position;
        velocity = rb.velocity;
    }
}
