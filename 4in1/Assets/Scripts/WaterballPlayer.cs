using UnityEngine;
using Mirror;

public class WaterballPlayer : CITEPlayer {
    [SyncVar(hook = nameof(OnHorizontalRotationChanged))]
    private Quaternion horizontalRotation;
    
    [SyncVar(hook = nameof(OnVerticalRotationChanged))] 
    private Quaternion verticalRotation;

    public GameObject barrelPart;
    public GameObject towerPart;

    private bool isRotating = false;
    private Vector3 initialMousePosition;
    private Quaternion initialRotation;

    private Quaternion initialTowerRotation;
    private Quaternion initialBarrelRotation; 

    // Start is called before the first frame update
    public override void OnStartLocalPlayer() {
        Debug.Log("Local GameBehaviour started as player " + playerID);

        // Find camera helpers and let them know who we are
        foreach (CameraPositioner helper in FindObjectsOfType<CameraPositioner>()) {
            helper.setView(playerID);
        } 
        isRotating = false;
    }

    public override void OnStartClient() {
        // if (!hasAuthority) {
            // transform.rotation = horizontalRotation;
        // }

        // initialRotation = transform.rotation;
    }

    private void OnHorizontalRotationChanged(Quaternion oldRotation, Quaternion newRotation) {
        towerPart.transform.localRotation = newRotation;
    }
    
    private void OnVerticalRotationChanged(Quaternion oldRotation, Quaternion newRotation)
    {
        barrelPart.transform.localRotation = newRotation;
    }


    public void ApplyForceOnBall(NetworkIdentity ballNetworkIdentity, Vector3 impactForce, Vector3 impactPosition) {
        if (hasAuthority) {
            CmdApplyForceOnBall(ballNetworkIdentity.netId, impactForce, impactPosition);
        }
    }

    [Command]
    private void CmdApplyForceOnBall(uint ballNetId, Vector3 impactForce, Vector3 impactPosition) {

        
        GameObject ballObject = NetworkServer.spawned[ballNetId].gameObject;
        WaterballBall ball = ballObject.GetComponent<WaterballBall>(); 
        
        ball.ApplyForce(impactForce, impactPosition);
        Debug.Log("inne i waterball player cmd apply force");
    }

    [Client]
    public void CmdRotate(float deltaY, float deltaX) {
        if (hasAuthority) {
            Quaternion horizontalRotation = Quaternion.Euler(0f, -deltaY, 0f);
            Quaternion newTowerRotation = initialTowerRotation * horizontalRotation;

            Quaternion verticalRotation = Quaternion.Euler(-deltaX, 0f, 0f);
            Quaternion newBarrelRotation = initialBarrelRotation * verticalRotation;

            // Quaternion barrelHorizontalRotation = Quaternion.Euler(0, deltaY, 0);
            // Quaternion newerBarrelRotation = newBarrelRotation * barrelHorizontalRotation; 

            CmdSetRotation(newTowerRotation, newBarrelRotation);
        }
    }


    [Command]
    public void CmdSetRotation(Quaternion newTowerRotation, Quaternion newBarrelRotation) {
        horizontalRotation = newTowerRotation;
        towerPart.transform.localRotation = newTowerRotation;

        verticalRotation = newBarrelRotation;
        barrelPart.transform.localRotation = newBarrelRotation; 
    }


    public void Update() {
        if (hasAuthority) {
            if (Input.GetMouseButtonDown(0)) {
                isRotating = true;
                initialMousePosition = Input.mousePosition;
                initialTowerRotation = towerPart.transform.localRotation;
                initialBarrelRotation = barrelPart.transform.localRotation; 
                // initialRotation = transform.rotation;
            }

            else if (Input.GetMouseButtonUp(0)) {
                isRotating = false;
            }

            if (isRotating) {
                Vector3 currentMousePosition = Input.mousePosition;
                float deltaX = (currentMousePosition.x - initialMousePosition.x) * 0.1f;
                float deltaY = (currentMousePosition.y - initialMousePosition.y) * 0.1f;
                CmdRotate(deltaX, deltaY);
            }
        }
    }
}



// public GameObject serverBasedTestObject;
// public GameObject clientBasedTestObject;

// private GameObject clientCube;
// private Transform towerTransform; 


// public void CmdMoveForward() {
// transform.Translate(Vector3.forward *Time.deltaTime);
// }


// if(hasAuthority)
// {
//     CmdCreateClientControlledTestObject(new Vector3(0, 0, 0));
// }


/**
    A client can ask the server to spawn a test object that is controlled entirely by the
    server and has the state of it broadcast to all of the clients
*/
// [Command] public void CmdCreateServerControlledTestObject(){
//     Debug.Log("Creating a test object");
//     
//     GameObject testThing = Instantiate(serverBasedTestObject, new Vector3(clientCube.transform.position.x, clientCube.transform.position.y, clientCube.transform.position.z), Quaternion.identity);
//     testThing.GetComponent<Rigidbody>().isKinematic = false; // We simulate everything on the server so only be kinematic on the clients
//     testThing.GetComponent<Rigidbody>().velocity = new Vector3(5,0,5);
//
//     // Tell everyone about this new shiny object
//     NetworkServer.Spawn (testThing);
// }
//
// /**
//     A client can request that the server spawns an object that the client can control directly
// */
// [Command] public void CmdCreateClientControlledTestObject(Vector3 initialPosition){
//     Debug.Log("Creating a test object for "+connectionToClient+" to control");
//     GameObject testThing = Instantiate(clientBasedTestObject, initialPosition, Quaternion.identity);
//
//     // Tell everyone about it and hand it over to the client who asked for it
//     NetworkServer.Spawn(testThing, connectionToClient);
//
//     clientCube = testThing;
// }