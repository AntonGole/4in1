using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CITEDiscovery;

public class LobbyBehaviour : MonoBehaviour
{
    [System.Serializable]
    public class Room {
        public string name;
        public GameObject[] stuffToShow;
    }
    public GameObject[] welcomeStuff;
    public Room[] rooms;
    public GameObject[] peopleIndicators;
    private CITERoomManager roomManager;

    public void Start() {
        // Get welcome screen ready
        roomManager = FindObjectOfType<CITERoomManager>();
        goToWelcome();

        // Update people indicator when number of people changes
        roomManager.roomChangeListeners += onPeopleIndicatorChanged;
        roomManager.joiningServerListeners += onJoiningServer;
    }

    public void OnDestroy(){
        if (roomManager!=null){
            roomManager.roomChangeListeners -= onPeopleIndicatorChanged;
	        roomManager.joiningServerListeners -= onJoiningServer;
        }
    }

    public void goToWelcome()
    {
        // Show welcome, leave any rooms
        showWelcome(true);
        roomManager.LeaveRoom();

        // Hide the people indicator
        onPeopleIndicatorChanged(null);
    }

    private void showWelcome(bool shouldShow)
    {
        // Hide or show welcome
        foreach (GameObject uiElement in welcomeStuff){
            uiElement.SetActive(shouldShow);
        }

        // Potentially hide all room screens
        if (shouldShow){
            foreach (Room room in rooms){
                foreach (GameObject uiElement in room.stuffToShow){
                    uiElement.SetActive(false);
                }
            }
        }
    }

    public void goToRoom(int roomID){
        // Hide the welcome screen
        showWelcome(false);

        // Show the room screen
        foreach (GameObject uiElement in rooms[roomID].stuffToShow){
            uiElement.SetActive(true);
        }

        // Join the room waiting queue
        roomManager.JoinRoom(rooms[roomID].name);
    }

    private void onPeopleIndicatorChanged(List<CITEDiscoveryPeer> roomPeers){
        int peers = (roomPeers==null?-1:roomPeers.Count);
        int i = 0;
        foreach (GameObject uiElement in peopleIndicators){
            uiElement.SetActive(peers>=i);
            i++;
        }
    }

    public void forceCreateServer(){
        Debug.Log("TODO");
	    //roomBrowser.forceCreateServer();
    }

    private void onJoiningServer(CITEDiscoveryPeer serverPeer){	    
        onPeopleIndicatorChanged(null); // Hide the indicator
        foreach (Room room in rooms) {
            foreach (GameObject uiElement in room.stuffToShow){
                uiElement.SetActive(false);
            }
        }
	    // TODO: Trigger the joining graphics
    }
}
