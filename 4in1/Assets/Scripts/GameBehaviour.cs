using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameBehaviour : CITEPlayer {
    public GameObject serverBasedTestObject;
    public GameObject clientBasedTestObject;

    private GameObject clientCube;

    // Start is called before the first frame update
    public override void OnStartLocalPlayer(){
        Debug.Log("Local GameBehaviour started as player "+playerID);

        // Find camera helpers and let them know who we are
        foreach (CameraPositioner helper in FindObjectsOfType<CameraPositioner>()){
            helper.setView(playerID);
        }

        if(hasAuthority)
        {
            CmdCreateClientControlledTestObject(new Vector3(0, 0, 0));
        }
    }

    /**
        A client can ask the server to spawn a test object that is controlled entirely by the
        server and has the state of it broadcast to all of the clients
    */
    [Command] public void CmdCreateServerControlledTestObject(){
        Debug.Log("Creating a test object");
        
        GameObject testThing = Instantiate(serverBasedTestObject, new Vector3(clientCube.transform.position.x, clientCube.transform.position.y, clientCube.transform.position.z), Quaternion.identity);
        testThing.GetComponent<Rigidbody>().isKinematic = false; // We simulate everything on the server so only be kinematic on the clients
        testThing.GetComponent<Rigidbody>().velocity = new Vector3(5,0,5);

        // Tell everyone about this new shiny object
        NetworkServer.Spawn (testThing);
    }

    /**
        A client can request that the server spawns an object that the client can control directly
    */
    [Command] public void CmdCreateClientControlledTestObject(Vector3 initialPosition){
        Debug.Log("Creating a test object for "+connectionToClient+" to control");
        GameObject testThing = Instantiate(clientBasedTestObject, initialPosition, Quaternion.identity);

        // Tell everyone about it and hand it over to the client who asked for it
        NetworkServer.Spawn(testThing, connectionToClient);

        clientCube = testThing;
    }

    public void Update(){
        if (hasAuthority){
            if (Input.GetKeyUp("space")){
                CmdCreateServerControlledTestObject();
            } else if (Input.GetKeyUp("m")){
            }
        }
    }
}
