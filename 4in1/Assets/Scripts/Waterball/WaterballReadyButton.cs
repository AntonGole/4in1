using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterballReadyButton : MonoBehaviour {


    public Sprite notReadyButton;
    public Sprite readyButton;


    private bool pushed = false; 
    
    private int playerID;

    private Sprite currentButton; 

    private void Start() {
        playerID = GetComponent<WaterballPlayer>().playerID;
        currentButton = notReadyButton; 
    }   


    private void Update() {
        var scene = SceneManager.GetActiveScene().name;
        if (scene != "Title Screen") {
            return; 
        }

        var position = calculateButtonPosition(playerID);
        var rotation = calculateButtonRotation(playerID);

        
        
        if (pushed) {
            var button = readyButton; 
        }
        else {
            var button = notReadyButton; 
        }
        
        
        









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
    
    
    
    
    
}