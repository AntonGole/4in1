using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using static CITEDiscovery;

public class CITERoomManager : MonoBehaviour {
    // Room management settings    
    public int minRequirement = 4;
    private List<CITEDiscoveryPeer> roomPeers; // Filtered list of peers found in the selected room
    private readonly object syncLock = new object();
    int roomStartState;
    public int initialJoinTimeout = 5;
    bool roomChanged = false;
    
    // Low-level discovery API
    private CITEDiscovery discovery;
    private CITEDiscoveryPeer broadcastInfo; // The peer info that others will see broadcasted from us

    // Callbacks for the UI, etc.
    public delegate void OnRoomMembersChanged(List<CITEDiscoveryPeer> roomPeers); // Called when the list of people in the room changes
    public event OnRoomMembersChanged roomChangeListeners;
    public delegate void OnJoiningServer(CITEDiscoveryPeer serverPeer); // Called when a server has been elected and we are starting the process to join it
    public event OnJoiningServer joiningServerListeners;    
    public delegate void OnCreatingServer(); // Called when we have been elected as server and are starting the process to create it
    public event OnCreatingServer creatingServerListeners;    

    public void Start() {
        roomStartState = -1;

        Debug.Log("Creating Discovery");
        roomPeers = new List<CITEDiscoveryPeer>();
        discovery = new CITEDiscovery();
        broadcastInfo = discovery.getLocalPeer();

        // Set the information that will be seen by other peers
        broadcastInfo.DeviceName = SystemInfo.deviceName;
        broadcastInfo.Uuid = Guid.NewGuid().ToString();
    }

    public void OnDestroy(){
        Debug.Log("Destroying RoomManager!!");
        discovery.Destroy();
    }

    /**
     * Handle changes in the peerlist that we get from discovery, this includes all peers in
     * all rooms
     */
    private void OnPeerListUpdated(List<CITEDiscoveryPeer> allSeenPeers) {
        Debug.Log("Discovery got some peer(s): "+allSeenPeers.Count);        

        lock (syncLock){
            // Clean old list from peers who are no longer in this room or who disappeared
            if (roomPeers.RemoveAll(peer => (!broadcastInfo.RoomId.Equals(peer.RoomId)) || (!allSeenPeers.Contains(peer)))>0){
                roomChanged = true;
            }

            // Add new peers
            allSeenPeers.ForEach(peer => {
                if (peer.RoomId.Equals(broadcastInfo.RoomId) && !roomPeers.Contains(peer)){
                    Debug.Log("Peer joined room: "+peer);
                    roomPeers.Add(peer);
                    roomChanged = true;
                }
            });
        }      
    }

    public void Update(){
        if (roomStartState < 0){ // Room hasn't started yet
            lock(syncLock){
                if (roomPeers.Count>=minRequirement-1){ // (-1 since we are also a "peer" but not in the list)
                    // Enough peers seen to create the room, check if we should create the server
                    bool weRule = true;
                    roomPeers.ForEach(peer => {
                        if (peer.IsServer || peer.Uuid.CompareTo(broadcastInfo.Uuid)>0) weRule = false;
                    });
                    if (weRule){
                        // We are the coolest peer, start the server
                        Debug.Log("We are the server - waiting for people to join us");
                        if (creatingServerListeners!=null) {
                            creatingServerListeners();
                        }
                        ForceCreateServer();
                        roomStartState = initialJoinTimeout;
                    }
                }

                if (roomChanged){
                    if (roomChangeListeners!=null){
                        roomChangeListeners(roomPeers);
                    }
                    Debug.Log("Room counter updated: "+roomPeers.Count);
                    roomChanged = false;
                }

                // If a server is in the room, just plain join it before processing anything else
                foreach (CITEDiscoveryPeer peer in roomPeers){
                    if (peer.IsServer){
                        LeaveRoom();
                        Debug.Log("Joining server "+peer);
                        if (joiningServerListeners!=null){
                            joiningServerListeners(peer);
                            CITENetworkManager network = GetComponent<CITENetworkManager>();
                            network.networkAddress = peer.NetworkAddress;
                            network.StartClient();
                        }
                        roomStartState = 0;
                        return;
                    }
                }
            }
        }
        /* else if 
	     CITENetworkManager server = GetComponent<CITENetworkManager>();
	     if (roomStartTime > 0 && Time.time > roomStartTime + initialJoinTimeout){
                // We are server for room that has just started - do a check for peerCount after a short while just to make sure
                if (server.GetConnectionCount()<minRequirement-1){
                    Debug.Log("Not enough connected players on this server after initial join timeout, killing everything and restarting");
		    server.error();
                } else {
                    Debug.Log("Room started successfully, everyone is happy");
                    roomStartTime = -1;
                }
            } else if (roomStartTime!=0 && !AppleLocalMultiplayer.Session.IsSessionActive){
		Debug.Log("Session connection got dropped, restarting everything");
		server.error();
	    }
        }*/    

    } 

    /**
     * Switch to the given room and start broadcasting our presence
     */
    public void JoinRoom(string room){
        Debug.Log("Room manager joining room " + room + " with "+minRequirement+" minimal requirement");
        broadcastInfo.IsServer = false;        
        broadcastInfo.RoomId = room;
        roomStartState = -1;
        discovery.onPeerListUpdated += OnPeerListUpdated;
        discovery.resetSeenPeers();
        discovery.startBroadcasting();
        roomChanged = true;
    }

    /**
     * Leave all rooms, stop broadcasting our presence
     */
    public void LeaveRoom(){
        Debug.Log("Room manager leaving room " + broadcastInfo.RoomId);
        discovery.onPeerListUpdated -= OnPeerListUpdated;
        discovery.stopBroadcasting();
        lock (syncLock){
            roomPeers.Clear();
        }
    }

    /**
     * Switch to announcing server mode in the currently joined room, this will cause others to join us
     */
    public void ForceCreateServer(){
        GetComponent<CITENetworkManager>().StartHost();
        broadcastInfo.IsServer = true;
    }
}
