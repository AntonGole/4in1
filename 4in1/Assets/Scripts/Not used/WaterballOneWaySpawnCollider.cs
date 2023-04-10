using System;
using System.Collections.Generic;
using UnityEngine;

public class WaterballOneWaySpawnCollider : MonoBehaviour {

    private Collider oneWayCollider;
    private HashSet<GameObject> exitedObjects;

    private void Start() {
        exitedObjects = new HashSet<GameObject>();
        oneWayCollider = GetComponent<Collider>(); 
    }

    private void OnTriggerEnter(Collider other) {

        if (!exitedObjects.Contains(other.gameObject)) {
            Physics.IgnoreCollision(oneWayCollider, other, true);
        }
    }

    private void OnTriggerExit(Collider other) {
        exitedObjects.Add(other.gameObject); 
        Physics.IgnoreCollision(oneWayCollider, other, false);
        Debug.Log("triggade ontriggerexit!!");
    }
}
