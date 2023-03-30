using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class WaterballParticleStream : NetworkBehaviour {
    public float forceMagnitude = 1;
    public ParticleSystem particleSystem;

    private void Start() {
        // particleSystem = GetComponent<ParticleSystem>();
        var collisions = particleSystem.collision;
        collisions.enabled = true;
        collisions.sendCollisionMessages = true;
    }


    private void OnParticleCollision(GameObject other) {
        if (!hasAuthority) {
            return;
        }

        if (other.gameObject.layer != LayerMask.NameToLayer("Ball")) {
            return;
        }

        WaterballBall waterballBall = other.GetComponent<WaterballBall>();
        if (waterballBall == null) {
            return;
        }

        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        int numCollisionEvents = particleSystem.GetCollisionEvents(other, collisionEvents);
        foreach (var collision in collisionEvents) {
            var collisionVelocity = collision.velocity;
            var collisionIntersection = collision.intersection;
            waterballBall.CmdApplyForce(collisionVelocity * forceMagnitude, collisionIntersection);
        }
    }
}