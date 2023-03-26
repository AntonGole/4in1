using Mirror;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CITEPlayer : NetworkBehaviour {
    [SyncVar] public int playerID = -1;
    [SyncVar] private Quaternion rotation; 
    /**
     * Handle players on the server
     */
    public override void OnStartServer() {
        // Let the player know their ID by setting a SyncVar
        playerID = FindObjectOfType<CITENetworkManager>().GetPlayerID(connectionToClient);
        rotation = transform.rotation; 
    }
    
    
    
    public override void OnStartClient()
    {
        if (!hasAuthority)
        {
            transform.rotation = rotation;
        }
    }
    
    [Client]
    public void CmdRotate(float deltaY, float deltaX)
    {
        if (hasAuthority)
        {
            Quaternion horizontalRotation = Quaternion.Euler(0f, deltaX, 0f);
            Quaternion newTowerRotation = transform.rotation * horizontalRotation;

            Quaternion verticalRotation = Quaternion.Euler(deltaY, 0f, 0f);
            Quaternion newBarrelRotation = transform.rotation * verticalRotation;

            CmdSetRotation(newTowerRotation, newBarrelRotation);
        }
    }
    
    
    [Command]
    public void CmdSetRotation(Quaternion newTowerRotation, Quaternion newBarrelRotation)
    {
        rotation = newTowerRotation;
        transform.rotation = newTowerRotation;
    }
    
    
    
    
}
