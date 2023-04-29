using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class WaterballDramaticSplash : MonoBehaviour {


    public ParticleSystem subEmitter;


    public float threshold;

    private Random rd = new Random();


    public float spawningHeight;


    private void OnParticleCollision(GameObject other) {


        // Debug.Log("vi är inne i sub emitterns SPAWN");

        var collisionEvents = new List<ParticleCollisionEvent>();
        int numCollisionEvents = GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents); 

        if (numCollisionEvents <= 0) {
            return; 
        }

        if (rd.NextDouble() < threshold) {
            return; 
        }


        ParticleCollisionEvent collision = collisionEvents[0];

        Vector3 collisionPoint = collision.intersection;

        Vector3 emissionPoint = new Vector3(collisionPoint.x, spawningHeight, collisionPoint.y);


        var subEmitterInstance = Instantiate(subEmitter, emissionPoint, Quaternion.identity);
        subEmitterInstance.Play();
        Destroy(subEmitterInstance.gameObject, subEmitterInstance.main.startLifetime.constantMax);

    }

    // private void OnCollisionEnter(Collision collision) {
    //
    //
    //     if (rd.NextDouble() < threshold) {
    //         return; 
    //     }
    //
    //
    //     var collisionPosition = collision.
    //     
    //     var position = new Vector3(collision) 
    //     
    //     var subEmitterInstance = Instantiate(subEmitter)
    //     
    //     
    // }
}