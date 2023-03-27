using Mirror;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Threading;
using System.Threading.Tasks;
using System;
using Mirror.SimpleWeb;

public class CITENetworkManager : NetworkManager {
    private Dictionary<NetworkConnection,int> connectedIDs;
    public int bootstrapSceneBuildIndex;
    public int errorSceneBuildIndex;
    public GameObject floor; 
    
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
    public override void OnServerConnect(NetworkConnectionToClient conn){
        // Find the first free player ID not already in use
        int newPlayerID = 0;
        // Debug.Log($"NEW PLAYER ID ::::: {newPlayerID}");
        while (connectedIDs.ContainsValue(newPlayerID)){
            newPlayerID++;
        }
        connectedIDs.Add(conn, newPlayerID);
        
        

        Debug.Log("Added connection for player with ID " + newPlayerID);
        base.OnServerConnect(conn);


        // Debug.Log("printing connected IDSSSSSSSSSS");

        // foreach (var connectedID in connectedIDs) {
            // Debug.Log($"key: {connectedID.Key}, value: {connectedID.Value}");
        // }
        
    }

    /**
     * Remove people from the connection list when they leave the server run by this manager
     */
    public override void OnServerDisconnect(NetworkConnectionToClient conn){
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

    private int PAUSE_DIE_TIME = 5;
    private float whenPaused = -1;
    private CancellationTokenSource cancelSource = null;

    /**
     * Handle the application being paused/losing focus
     */
    public void OnApplicationPause(bool paused){
        if(cancelSource != null) {
            cancelSource.Cancel();
            cancelSource = null;
        }

        if(paused) {
            //We are now paused, remember when it happened
            whenPaused = Time.realtimeSinceStartup;

            Debug.Log("Paused...");

            cancelSource = new CancellationTokenSource();

            Task.Run(async () => {
                await Task.Delay(TimeSpan.FromSeconds(PAUSE_DIE_TIME), cancelSource.Token);
                //TODO Kill new transport?

                //LiteNetLib4MirrorTransport t = (LiteNetLib4MirrorTransport)transport;
                //t.Shutdown();
                Debug.Log("Transport killed !!!NOT!!!...");
            });

        } else {
            float pauseTime = Time.realtimeSinceStartup - whenPaused;

            //No longer paused, check if more time passed than we allow
            if (whenPaused > 0 && pauseTime > PAUSE_DIE_TIME) {
                Debug.Log("Application was paused more than " + PAUSE_DIE_TIME + "sec, rebooting everything... ("+(Time.realtimeSinceStartup - whenPaused) +"sec)");
                error();
            } else {
                Debug.Log("Came pack from small pause: " + pauseTime + "sec");
            }

            whenPaused = -1;
        }
    }

    /**
     * Handle being disconnected from a running game server
     */
    public override void OnClientDisconnect(){
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

    public override void OnServerAddPlayer(NetworkConnectionToClient conn) {
        
        Debug.Log($"NetworkConnectionToClient ----- ID: {conn.connectionId}, {conn.address}");


        base.OnServerAddPlayer(conn);

        GameObject go = conn.identity.gameObject;
        go.transform.position = calculateCornerPosition(GetPlayerID(conn), floor, go);
        Quaternion rot =  calculateCornerRotation(GetPlayerID(conn));
        Debug.Log($"I got rotation: {rot}");
        go.transform.rotation = calculateCornerRotation(GetPlayerID(conn));
        Debug.Log($"the rotation of the transform: {go.transform.rotation}");
        go.transform.Translate(Vector3.forward * 1.5f);
        Vector3 direction_towards_center = new Vector3(0, 0, 0) - go.transform.position;
        go.transform.rotation = Quaternion.LookRotation(direction_towards_center); 
        conn.identity.AssignClientAuthority(conn);
    }

    private Vector3 calculateCornerPosition(int player_identity, GameObject floor, GameObject player) {
        
        Debug.Log($"player_identity: {player_identity}");

        MeshRenderer renderer = floor.GetComponent<MeshRenderer>(); 
        // Vector3[] corners = new Vector3[8];
        Bounds bounds = renderer.bounds; 
        // renderer.bounds.extents.x, 

        var delta_x = bounds.extents.x;
        var delta_y = bounds.extents.y;
        var delta_z = bounds.extents.z;
        var player_height = player.GetComponent<MeshRenderer>().bounds.extents.y; 

        Vector3 adjust_up = new Vector3(0, player_height, 0);

        Vector3 corner0 = bounds.center + new Vector3(-delta_x, delta_y, delta_z) + adjust_up;
        Vector3 corner1 = bounds.center + new Vector3(delta_x, delta_y, delta_z) + adjust_up;
        Vector3 corner2 = bounds.center + new Vector3(-delta_x, delta_y, -delta_z) + adjust_up;
        Vector3 corner3 = bounds.center + new Vector3(delta_x, delta_y, -delta_z) + adjust_up;
        Vector3 default_position = bounds.center + new Vector3(0, delta_y, 0) + adjust_up;

        
        switch (player_identity) {
            case 0:
                return corner0;
                break; 
            case 1:
                return corner1;
                break; 
            case 2:
                return corner2;
                break; 
            case 3:
                return corner3;
                break; 
            default:
                return default_position; 
            }

    }

    private Quaternion calculateCornerRotation(int player_identity) {

        Debug.Log($"inne i calculate corner!!!!!!!! player identity: {player_identity}");
        
        switch (player_identity) {
            case 0:
                return Quaternion.Euler(0f, 135f, 0f); 
                break; 
            case 1:
                return Quaternion.Euler(0f, -135f, 0f); 
                break; 
            case 2:
                return Quaternion.Euler(0f, 45, 0f); 
                break; 
            case 3:
                return Quaternion.Euler(0f, -45f, 0f); 
                break; 
            default:
                return Quaternion.identity; 
        }
    }
    
}


        
        
// Vector3[] hello = new Vector3[4]; 
            
            

        
// floor.transform.TransformPoint(cor)



// float delta_x = 20 / 2;
// float delta_z = (float)(16.2 / 2);
// var object_width = 1;
//
// // Vector3 output = new Vector3(0, 0, 0); 
//
// switch (player_identity) {
//     case 0:
//         return new Vector3(-delta_x + 0.5f, 0, delta_z - 0.5f);
//         break; 
// default:
//     return new Vector3(0, 0, 0); 
// }
// Vector3[] hello = new Vector3[4]; 


