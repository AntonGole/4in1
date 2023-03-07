using Mirror;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CITEPlayer : NetworkBehaviour {
    [SyncVar] public int playerID = -1;

    /**
     * Handle players on the server
     */
    public override void OnStartServer() {
        // Let the player know their ID by setting a SyncVar
        playerID = FindObjectOfType<CITENetworkManager>().GetPlayerID(connectionToClient);
    }
}
