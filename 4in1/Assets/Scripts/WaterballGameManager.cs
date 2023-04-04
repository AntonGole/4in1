using System;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace {
    public class WaterballGameManager : NetworkBehaviour {



        public string[] levelNames;  
        

        [SyncVar] 
        private int currentLevel = 0;

        

        private void Start() {
            levelNames =  new string[] {"GameScene", "Level 1", "Level 2" };
        }

        private void Update() {

            if (SceneManager.GetActiveScene().name == "Network") {

                Debug.Log("vi är i network");
                return; 
            }

            if (SceneManager.GetActiveScene().name == "LobbyScene") {
                Debug.Log("vi är i lobby");
                return; 
            }
            
            if (SceneManager.GetActiveScene().name == "ErrorScene") {
                Debug.Log("vi är i error");
                return; 
            }


            // Debug.Log("vi är någonstans i gamet!");


            if (Input.GetKeyDown(KeyCode.K)) {
                currentLevel++;
                if (currentLevel >= levelNames.Length) {
                    currentLevel = 0;
                }

                Debug.Log(currentLevel);
                Debug.Log(levelNames);
                Debug.Log(levelNames[currentLevel]);
                GetComponent<CITENetworkManager>().ServerChangeScene(levelNames[currentLevel]);
            }
        }
    }
}