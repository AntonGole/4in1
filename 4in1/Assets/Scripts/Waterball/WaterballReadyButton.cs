using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Mirror; 

public class WaterballReadyButton : MonoBehaviour {
    
    public Sprite notReadyButton;
    public Sprite readyButton; 
    public Button button; 
    private Sprite currentSprite;
    private Transform buttonTransform;

    public GameObject parentNetworkObject; 
    
    // [SyncVar(hook = nameof(OnButtonStateChanged))]
    private bool pushed = false;

    private int playerID; 

    // public WaterballReadyButton(int playerID) {
        // this.playerID = playerID; 
    // }


    // public void setPlayerID(int playerID) {
        // this.playerID = playerID; 
    // }


    private void Awake() {
        
        button.onClick.AddListener(OnButtonClick);
        
        
    }

    private void Start() {
        currentSprite = notReadyButton; 
        playerID = parentNetworkObject.GetComponent<WaterballPlayer>().playerID; 
        
    }

    private void OnButtonClick() {
        // if (isServer) {
            pushed = !pushed;
        // }
        
        if (pushed) {
            button.image.sprite = readyButton;
            // WaterballGameManager.Instance.IncrementReadyPlayers();
        }
        else {
            button.image.sprite = notReadyButton;
            // WaterballGameManager.Instance.DecrementReadyPlayers();
        }

        // var message = new WaterballReadyButtonMessage {
            // playerID = playerID,
            // IsReady = pushed
        // };
        
        parentNetworkObject.GetComponent<WaterballPlayer>().TellServerReadyButtonPressed(pushed);
        
    }
    
    
    // private void OnButtonStateChanged(bool oldState, bool newState) {
    //     button.image.sprite = newState ? readyButton : notReadyButton;
    // }
    //
    
    
    
    
}