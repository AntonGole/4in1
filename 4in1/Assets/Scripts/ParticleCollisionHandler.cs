using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ParticleCollisionHandler : MonoBehaviour {
    public ParticleSystem normalSubEmitterPrefab;
    public ParticleSystem dramaticSubEmitterPrefab;

    public float splashThreshold = 0.7f;
    public float dramaticThreshold = 0.9f;
    public string ballLayerName = "Ball";

    private Random rnd = new Random();

    private void OnParticleCollision(GameObject other) {
        var collisionEvents = new List<ParticleCollisionEvent>();
        int numCollisionEvents = GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);

        if (numCollisionEvents <= 0) {
            return;
        }

        // only play collision effect sometimes
        if (rnd.NextDouble() < splashThreshold) {
            return;
        }

        ParticleCollisionEvent collision = collisionEvents[0];
        Vector3 collisionPoint = collision.intersection;
        Vector3 normal = collision.normal;
        ParticleSystem subEmitterInstance;
        Vector3 direction;
        ParticleSystem emitter;

        if (other.gameObject.layer == LayerMask.NameToLayer(ballLayerName) && (rnd.NextDouble() > dramaticThreshold)) {
            direction = (normal + Vector3.up).normalized;
            emitter = dramaticSubEmitterPrefab;
        }
        else {
            direction = normal;
            emitter = normalSubEmitterPrefab;
        }

        subEmitterInstance = Instantiate(emitter, collisionPoint, Quaternion.LookRotation(direction));
        subEmitterInstance.Play();
        Destroy(subEmitterInstance.gameObject, subEmitterInstance.main.startLifetime.constantMax);
        Debug.Log(subEmitterInstance.main.startLifetime);
    }
}