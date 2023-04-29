using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaterballCanvasManager : MonoBehaviour {

    public static WaterballCanvasManager Instance;
    
    public Canvas canvas;
    public GameObject waterballReadyButtonPrefab; 
    
    
    private void Awake() {
        if (Instance == null) {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }

        else {
            if (Instance != this) {
                Destroy(gameObject);
            }
        }
        
    }

    
    private void Start() {
        // canvas = GetComponent<Canvas>(); 



        // var go = gameObject; 

        // Debug.Log("this is a gameobject");
        // Debug.Log(go);
        // GetComponent<GameObject>().SetActive(false);
        
        gameObject.SetActive(false);
    }   
    
   

    
    public void LoadSceneUI(int playerID) {
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

        var readyButton = Instantiate(waterballReadyButtonPrefab, position, rotation, canvas.transform); 
        readyButton.transform.localPosition = position;
        readyButton.transform.localScale = scale;
        
        var script = readyButton.GetComponent<WaterballReadyButton>();
        script.setPlayerID(playerID);
    }
    
    


    private void HideCanvas() {
        gameObject.SetActive(false); 
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
    
    
    
    


    private void Update() {


    }


    
    
    
    
    
}


// var scene = SceneManager.GetActiveScene().name;
// if (scene != "Title Screen") {
//     return; 
// }
//
// var position = calculateButtonPosition(playerID);
// var rotation = calculateButtonRotation(playerID);
//
//
//
// if (pushed) {
//     currentSprite = readyButton; 
// }
// else {
//     currentSprite = notReadyButton; 
// }
//
// button.GetComponent<Image>().sprite = currentSprite;
//
// // var buttonTransform = button.transform; 
// buttonTransform.position = position;
// buttonTransform.rotation = rotation; 





// Debug.Log("trying ID");
// playerID = GetComponent<WaterballPlayer>().playerID;
// Debug.Log("trying notreadybutton");
//
// currentSprite = notReadyButton;
// Debug.Log("trying transform");
//
// buttonTransform = button.transform; 

