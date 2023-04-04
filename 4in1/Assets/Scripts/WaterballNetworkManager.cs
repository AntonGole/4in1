using Mirror;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Threading;
using System.Threading.Tasks;
using System;
using Mirror.SimpleWeb;

public class WaterballNetworkManager : CITENetworkManager {
    private Dictionary<NetworkConnection,int> connectedIDs;
    public GameObject floor; 
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn) {
        
        // Debug.Log($"NetworkConnectionToClient ----- ID: {conn.connectionId}, {conn.address}");


        // base.OnServerAddPlayer(conn);

        int playerId = GetPlayerID(conn);
        // var gameObject = conn.identity.gameObject; 

        Vector3 middlePosition = new Vector3(0, 0, 0);
        Vector3 cornerPosition = calculateCornerPosition(playerId, floor, playerPrefab);
        Quaternion cornerRotation = calculateCornerRotation(playerId);
        Vector3 nudgedPosition = calculateNudgedPosition(cornerPosition, cornerRotation, 0.8f);
        Quaternion nudgedRotation = calculateNudgedRotation(cornerPosition, middlePosition);


        Debug.Log($"playerid: {playerId}, nudged position: {nudgedPosition}, nudged rotation: {nudgedRotation}");
        
        // Transform startPos = GetStartPosition();

        // GameObject player = startPos != null
        // ? Instantiate(playerPrefab, nudgedPosition, nudgedRotation)
        // : Instantiate(playerPrefab);


        GameObject player = Instantiate(playerPrefab, nudgedPosition, nudgedRotation);

        // instantiating a "Player" prefab gives it the name "Player(clone)"
        // => appending the connectionId is WAY more useful for debugging!
        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";

        NetworkServer.AddPlayerForConnection(conn, player);
        conn.identity.AssignClientAuthority(conn);
        
        
                
        //
        //
        //
        // GameObject go = conn.identity.gameObject;
        // go.transform.position = calculateCornerPosition(GetPlayerID(conn), floor, go);
        // Quaternion rot =  calculateCornerRotation(GetPlayerID(conn));
        // // Debug.Log($"I got rotation: {rot}");
        // go.transform.rotation = calculateCornerRotation(GetPlayerID(conn));
        // // Debug.Log($"the rotation of the transform: {go.transform.rotation}");
        // go.transform.Translate(Vector3.forward * 1.5f);
        // Vector3 direction_towards_center = new Vector3(0, 0, 0) - go.transform.position;
        // go.transform.rotation = Quaternion.LookRotation(direction_towards_center); 
        //
        //
        //
        
        
        
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

    private Vector3 calculateNudgedPosition(Vector3 cornerPosition, Quaternion orientation, float nugdeDistance) {
        return cornerPosition + orientation * Vector3.forward * nugdeDistance; 
    }

    private Quaternion calculateNudgedRotation(Vector3 position, Vector3 middle) {
        Vector3 directionToMiddle = middle - position;
        return Quaternion.LookRotation(directionToMiddle); 
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


