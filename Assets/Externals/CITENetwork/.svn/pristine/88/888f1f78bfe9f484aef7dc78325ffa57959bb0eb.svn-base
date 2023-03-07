using Mirror;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CITENetworkManager : NetworkManager {
    private Dictionary<NetworkConnection,int> connectedIDs;
    public int bootstrapSceneBuildIndex;
    public int errorSceneBuildIndex;

    public override void Awake(){
        base.Awake();
	    initialize();
    }

    public void initialize(){
        Debug.Log("Loading bootstrap");
        Application.targetFrameRate = 60;
        connectedIDs = new Dictionary<NetworkConnection, int>();
        SceneManager.LoadScene(bootstrapSceneBuildIndex, LoadSceneMode.Single);
    }

    public override void OnDestroy(){
        base.OnDestroy();
        StopHost(); // De-allocate network sockets etc immediately on destruction
    }

    /**
     * Perform an action when someone joins the server run by this network manager
     */
    public override void OnServerConnect(NetworkConnection conn){
        // Find the first free player ID not already in use
        int newPlayerID = 0;
        while (connectedIDs.ContainsValue(newPlayerID)){
            newPlayerID++;
        }
        connectedIDs.Add(conn, newPlayerID);

        Debug.Log("Added connection for player with ID " + newPlayerID);
        base.OnServerConnect(conn);
    }

    /**
     * Remove people from the connection list when they leave the server run by this manager
     */
    public override void OnServerDisconnect(NetworkConnection conn){
        Debug.Log("Player " + connectedIDs[conn] + " disconnected");
        connectedIDs.Remove(conn);

        // For now we intentionally crash everything since disconnection handling is not part of the API yet
	    Debug.Log("Someone disconnected, rebooting everything...");
        error();
    }

    /** 
     * Lookup a player's ID using the connection
     */
    public int GetPlayerID(NetworkConnection conn){
        return connectedIDs[conn];
    }

    /**
     * Counts the total number of players currently connected
     */
    public int GetConnectionCount(){
        return connectedIDs.Count;
    }

    /**
     * Handle the application being paused/losing focus
     */
    public void OnApplicationPause(bool paused){
#if !UNITY_EDITOR
        // For now we intentionally crash the application since pausing is not yet part of the API
	    Debug.Log("Application was paused, rebooting everything...");
	    error();
#endif
    }

    /**
     * Handle being disconnected from a running game server
     */
    public override void OnClientDisconnect(NetworkConnection connection){
        // For now we intentionally crash the application since being kicked is not yet part of the API
    	Debug.Log("This client was disconnected, rebooting everything...");
    	error();
    }

    /**
     * An error occoured, handle it
     */
    public void error(){
	    Debug.Log("------------------------->>> REBOOT <<<-----------------------------------");
        SceneManager.LoadScene(errorSceneBuildIndex, LoadSceneMode.Single);
    }
}
