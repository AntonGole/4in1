using System;
using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace {
    public class WaterballGameManager : NetworkBehaviour {
        public GameObject bannerPrefab; 

        public string[] levelNames;
        
        private bool isPlayingBanner = false;
        private GameObject bannerInstance; 
        
        
        [SyncVar] 
        private int currentLevel = 0;

        [SyncVar] 
        private GameState currentState = GameState.Warmup; 

        public enum GameState {
            Warmup, 
            BallSpawning, 
            Playing, 
            EndingLevel
        }
        

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

            if (!isServer) {
                Debug.Log("vi är inte server");
                return; 
            }


            switch (currentState) {
                case GameState.Warmup:

                    if (!isPlayingBanner) {
                        ShowGetReadyBanner(); 
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
                bannerInstance = null; 
                // currentState = GameState.Warmup;
                // Debug.Log("vi ska va i warmup igen nu!");
                // Debug.Log($"isPlayingBanner {isPlayingBanner}");
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
        


        private void ShowGetReadyBanner() {
            isPlayingBanner = true; 
            // Debug.Log("före krasch");
            if (bannerInstance == null) {
                // Debug.Log("entered instantiate igen!!");
                bannerInstance = Instantiate(bannerPrefab);
                Debug.Log(bannerInstance);
            }
            StartCoroutine(DisplayBannerForSomeTime(bannerInstance, 4f));
            // Debug.Log("efter coroutine");
            
            
        }


        private IEnumerator DisplayBannerForSomeTime(GameObject banner, float time) {
            // GameObject banner = GameObject.FindGameObjectWithTag("Circle Banner"); 
            // Debug.Log("räknar ner");
            banner.SetActive(true);
            Debug.Log(banner);
            yield return new WaitForSeconds(time);
            currentState = GameState.BallSpawning;
            // Debug.Log("nu är jag klar!!");
            banner.SetActive(false);
            Debug.Log(banner);
            isPlayingBanner = false; 
        }
        
        
        
        
    }
}