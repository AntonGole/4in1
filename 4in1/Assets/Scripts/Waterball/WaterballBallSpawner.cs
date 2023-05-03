using UnityEngine;
using Mirror;
using System.Collections;
using Random = System.Random;


public class WaterballBallSpawner : NetworkBehaviour {
    public GameObject ballPrefab;
    public GameObject oneWaySpawnCollider;
    public float oneWayColliderTimeActive = 3f;
    public float minBallSpawningSpeed = 15f;
    public int numberOfBalls = 1;
    public float spawningHeight = 8f;

    [SyncVar(hook = nameof(OnColliderStateChanged))]
    public bool isOneWayColliderActive;
    
    private Random rd = new Random();



    private void OnColliderStateChanged(bool oldValue, bool newValue) {
        // oneWaySpawnCollider.GetComponent<Collider>().isTrigger = !newValue;
        oneWaySpawnCollider.SetActive(newValue); 
        // oneWaySpawnCollider.GetComponent<Collider>().enabled = newValue; 
    }
    
    [Server]
    public IEnumerator SpawnBalls() {
        // Debug.Log("script börjar");
        // oneWaySpawnCollider.GetComponent<Collider>().enabled = true;
        isOneWayColliderActive = true; 
        Vector3 position = new Vector3(0, spawningHeight, 0);
        int counter = numberOfBalls; 
        while (counter > 0) {
            Quaternion ballDirection = GetRandomBallDirection();
            GameObject ballInstance = Instantiate(ballPrefab, position, ballDirection);
            Vector3 velocity = GetRandomBallVelocity(minBallSpawningSpeed, ballInstance);
            ballInstance.GetComponent<Rigidbody>().velocity = velocity;
            NetworkServer.Spawn(ballInstance);
            position = GetNextBallSpawnPosition(position);
            counter--;
        }

        // Debug.Log("Spawned balls!");
        yield return new WaitForSeconds(oneWayColliderTimeActive);
        // Debug.Log("Removing invisible walls!");
        // var collider = oneWaySpawnCollider.GetComponent<Collider>();
        // Debug.Log(collider);
        // collider.enabled = false; 
        // oneWaySpawnCollider.GetComponent<Collider>().enabled = false;
        isOneWayColliderActive = false; 
        // Debug.Log("script slutar");
    }

    private Vector3 GetNextBallSpawnPosition(Vector3 lastPosition) {
        var ballHeight = ballPrefab.GetComponent<Renderer>().bounds.size.y; 
        return lastPosition + Vector3.up * (ballHeight + 0.1f);
    }

    private Quaternion GetRandomBallDirection() {
        float multiplier = (float) rd.NextDouble();
        Quaternion rotationY = Quaternion.Euler(0, 360 * multiplier, 0);
        Quaternion rotationX = Quaternion.Euler(0, 0, 0);
        Quaternion finalRotation = rotationY * rotationX;
        return finalRotation;
    }

    private Vector3 GetRandomBallVelocity(float minBaseSpeed, GameObject ballInstance) {
        float forwardRoll = (float) rd.NextDouble();
        Vector3 velocity = ballInstance.transform.forward * (forwardRoll + 1) * minBaseSpeed;
        // Debug.Log(velocity);
        return velocity;
    }
}