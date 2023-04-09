using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class WaterballParticleStreamPhysicsInteraction : MonoBehaviour {
    public float forceMagnitude = 1;
    public ParticleSystem particleSystem;  // I get error here, add "new"?
    public GameObject parentNetworkObject;
    private NetworkIdentity parentNetworkIdentity; 

    private void Start() {
        // particleSystem = GetComponent<ParticleSystem>();
        var collisions = particleSystem.collision;
        collisions.enabled = true;
        collisions.sendCollisionMessages = true;
        parentNetworkIdentity = parentNetworkObject.GetComponent<NetworkIdentity>(); 
    }


    private void OnParticleCollision(GameObject other) {
        if (!parentNetworkIdentity.hasAuthority) {
            // Debug.Log("hade inte authority");
            return;
        }

        if (other.gameObject.layer != LayerMask.NameToLayer("Ball")) {
            // Debug.Log("layermasken var inte Ball");
            return;
        }

        WaterballBall waterballBall = other.GetComponent<WaterballBall>();
        if (waterballBall == null) {
            // Debug.Log("fanns ingen waterball component");
            return;
        }

        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        int numCollisionEvents = particleSystem.GetCollisionEvents(other, collisionEvents);

        if (numCollisionEvents > 0) {
            // Debug.Log("found collisions");
        }
        else {
            // Debug.Log("fanns inga collisions");
        }

        foreach (var collision in collisionEvents) {
            var collisionVelocity = collision.velocity;
            var collisionIntersection = collision.intersection;
            var forceApplied = collisionVelocity * forceMagnitude; 
            parentNetworkObject.GetComponent<WaterballPlayer>()
                .ApplyForceOnBall(other.GetComponent<NetworkIdentity>(), forceApplied, collisionIntersection);
        }
    }
}