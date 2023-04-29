using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WaterballPlayerHUD : MonoBehaviour {




    public GameObject readyButton;



    public GameObject parentNetworkObject; 
    
    private int playerID;


    private void Start() {
        playerID = parentNetworkObject.GetComponent<WaterballPlayer>().playerID; 
    }


    public void LoadSceneUI() {
        var scene = SceneManager.GetActiveScene().name;
        switch (scene) {
            case "Title Screen":
                Debug.Log("hello!!!! this is in loadSceneUI for title screen");
                ShowTitleScreenUI(playerID);
                return;
            default:
                HideCanvas();
                Debug.Log("just hide canvas");
                return; 
        }
        
        
        
    }
    
    
    private void ShowTitleScreenUI(int playerID) {
        gameObject.SetActive(true);
        var position = calculateButtonPosition(playerID);
        var rotation = calculateButtonRotation(playerID);
        var scale = new Vector3(1, 1, 1);

        // var readyButton = Instantiate(waterballReadyButtonPrefab, position, rotation, canvas.transform); 
        readyButton.transform.localPosition = position;
        readyButton.transform.localScale = scale;
        
        var script = readyButton.GetComponent<WaterballReadyButton>();
        // script.setPlayerID(playerID);
    }
    
    
    
    
    
    
        
    private Vector3 calculateButtonPosition(int playerID) {
        var x = 140;
        var y = 25;
        var z = 0;

        switch (playerID) {
            case 0:
                return new Vector3(x, -y, z);
            case 1:
                return new Vector3(-x, -y, z);
            case 2:
                return new Vector3(x, y, z);
            case 3:
                return new Vector3(-x, y, z);
            default:
                return new Vector3(0, 0, 0);

        }
    }

    private Quaternion calculateButtonRotation(int playerID) {
        
        switch (playerID) {
            case 0:
                return Quaternion.Euler(0, 0, 45);
            case 1:
                return Quaternion.Euler(0, 0, -45);
            case 2:
                return Quaternion.Euler(0, 0, 135);
            case 3:
                return Quaternion.Euler(0, 0, -135);
            default:
                return Quaternion.Euler(0, 0, 0);

        }
    }
    
    private void HideCanvas() {
        gameObject.SetActive(false); 
    }

    
    
    
    
    
    
}