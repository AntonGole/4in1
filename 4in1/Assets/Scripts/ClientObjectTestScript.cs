using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ClientObjectTestScript : NetworkBehaviour {
    [SyncVar(hook = "OnColourChanged")] public Color theColour; // all objects have a colour set by their owner
    private static Color ourColour;
    private static bool firstTime = true;

    private Vector3 moveTarget;
    private bool moveTargetChanged = false;

    public override void OnStartClient(){        
        // Tell our object to be our own colour when it spawns so we can recognize it
        if (hasAuthority){
            // The very first time we create a random colour to keep using when spawning new objects
            if (firstTime){
                 ourColour =  Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                 firstTime = false;
            }

            // Tell everyone about it through the SyncVar that we have authority over
            // This triggers OnColourChanged for everyone
            CmdPleaseChangeMyColour(ourColour);
        }
    }

    /**
     * Ask the server to change the colour of the object
     */
    [Command] public void CmdPleaseChangeMyColour(Color newColour){
        theColour = newColour;
    }

    /**
     * Called when someone changes colour on an object they own
     */
    public void OnColourChanged(Color oldColor, Color newColour){
        Material ourMaterial = new Material(Shader.Find("Standard"));
        ourMaterial.color = newColour;
        GetComponent<Renderer>().material = ourMaterial;
    }

    // Update is called once per frame
    void Update(){
        if(hasAuthority)
        {
            if(Input.GetMouseButton(0))
            {
                UpdatePosition(Input.mousePosition);
            } else if(Input.touchCount > 0)
            {
                UpdatePosition(Input.touches[0].position);
            }
        }
    }

    private void UpdatePosition(Vector3 inputPosition)
    {
        Camera cam = Camera.main;
        inputPosition.z = cam.transform.position.y;
        Vector3 moveTarget = cam.ScreenToWorldPoint(inputPosition);
        CmdSetMoveTarget(moveTarget);

        this.GetComponent<Rigidbody>().MovePosition(moveTarget);
    }

    [Command]
    private void CmdSetMoveTarget(Vector3 target)
    {
        moveTarget = target;
        moveTargetChanged = true;
    }

    private void FixedUpdate()
    {
        if (isServer)
        {
            if(moveTargetChanged)
            {
                moveTargetChanged = false;
                this.GetComponent<Rigidbody>().MovePosition(moveTarget);
            }
        }
    }
}
