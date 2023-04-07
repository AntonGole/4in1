using System;
using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace {
    public class WaterballGameManager : NetworkBehaviour {
        public GameObject bannerPrefab;
        public GameObject ballPrefab; 

        public string[] levelNames;

        private bool isPlayingBanner = false;
        // private GameObject bannerInstance;


        [SyncVar] private int currentLevel = 0;

        [SyncVar] private GameState currentState = GameState.Warmup;

        public enum GameState {
            Warmup,
            BallSpawning,
            Playing,
            EndingLevel
        }


        private void Start() {
            levelNames = new string[] {"GameScene", "Level 1", "Level 2"};
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

            if (!isServer) {
                Debug.Log("vi är inte server");
                return;
            }

            switch (currentState) {
                case GameState.Warmup:

                    if (!isPlayingBanner) {
                        // ShowGetReadyBanner();
                        
                    }

                    Debug.Log("warmup");
                    break;
                case GameState.BallSpawning:
                    Debug.Log("ball spawning");
                    break;
                case GameState.Playing:
                    Debug.Log("ball spawning");
                    break;
                case GameState.EndingLevel:
                    Debug.Log("ball spawning");
                    break;
                default:
                    Debug.Log("inget state");
                    break;
            }

            if (Input.GetKeyDown(KeyCode.K)) {
                currentLevel++;
                if (currentLevel >= levelNames.Length) {
                    currentLevel = 0;
                }

                Debug.Log(currentLevel);
                Debug.Log(levelNames);
                Debug.Log(levelNames[currentLevel]);
                GetComponent<CITENetworkManager>().ServerChangeScene(levelNames[currentLevel]);
                // bannerInstance = null;
            }

            if (Input.GetKeyDown(KeyCode.L)) {
                SpawnBall();
            }
            
            if (Input.GetKeyDown(KeyCode.O)) {
                SpawnBanner(); 
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            Debug.Log("New scene loaded: " + scene.name);
            if (scene.name != "Network" && scene.name != "LobbyScene" && scene.name != "ErrorScene") {
                currentState = GameState.Warmup;
            }
        }

        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }


        [Server]
        public void SpawnBall() {
            GameObject ballInstance = Instantiate(ballPrefab, new Vector3(0, 2, 0), Quaternion.identity); 
            NetworkServer.Spawn(ballInstance);
            Debug.Log("spawning a ball");
        }

        [Server]
        public void SpawnBanner() {
            GameObject bannerInstance = Instantiate(bannerPrefab); 
            NetworkServer.Spawn(bannerInstance);
            Debug.Log("spawning a banner");
        }
        
        
        
        


        // [ClientRpc]
        // private void ShowGetReadyBanner() {
        //     if (isPlayingBanner) {
        //         return;
        //     }
        //
        //     isPlayingBanner = true;
        //     StartCoroutine(SpawnBannerAndDisplay(4f));
        // }
        //
        //
        // private IEnumerator SpawnBannerAndDisplay(float displayTime) {
        //     yield return null;
        //
        //     if (bannerInstance == null) {
        //         bannerInstance = Instantiate(bannerPrefab);
        //         NetworkServer.Spawn(bannerInstance);
        //         Debug.Log(bannerInstance);
        //     }
        //     
        //     RpcDisplayBannerForSomeTime(displayTime);
        // }
        //
        //
        // [ClientRpc]
        // private void RpcDisplayBannerForSomeTime(float time) {
        //     if (bannerInstance == null) {
        //         return;
        //     }
        //
        //     StartCoroutine(DisplayBannerForSomeTime(bannerInstance, time));
        // }
        //
        //
        // private IEnumerator DisplayBannerForSomeTime(GameObject banner, float time) {
        //     banner.SetActive(true);
        //     Debug.Log(banner);
        //     yield return new WaitForSeconds(time);
        //     currentState = GameState.BallSpawning;
        //     banner.SetActive(false);
        //     Debug.Log(banner);
        //     isPlayingBanner = false;
        // }
    }
}


// {
//     // Find the GameObject with the PuzzleBehaviour script attached
//     PuzzleBehaviour puzzleBehaviour = FindObjectOfType<PuzzleBehaviour>();
//     if (puzzleBehaviour != null)
//     {
//         // Get the GameObject that has the PuzzleBehaviour script
//         GameObject puzzleGameObject = puzzleBehaviour.gameObject;
//
//         // Print the name of the GameObject
//         Debug.Log("PuzzleBehaviour script is attached to: " + puzzleGameObject.name);
//     }
//     else
//     {
//         Debug.Log("PuzzleBehaviour script not found in the scene.");
//     }
// }