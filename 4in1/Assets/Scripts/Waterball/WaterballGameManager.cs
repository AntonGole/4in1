using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;


namespace DefaultNamespace {
    public class WaterballGameManager : NetworkBehaviour {
        // public string[] levelNames;
        // private bool isPlayingGetReady = false;
        // private bool isEndingLevel = false;
        // private bool isSpawningBalls = false;
        // private bool isLoading = false;

        public List<string> levelNames; 

        private Coroutine warmupCoroutine; 
        private Coroutine ballSpawningCoroutine; 
        private Coroutine endingCoroutine; 
        
        
        
        

        private GameObject levelManager;

        private int currentLevel = 0;
        private GameState currentState = GameState.Loading;

        public enum GameState {
            Loading,
            Warmup,
            BallSpawning,
            Playing,
            EndingLevel
        }


        private void Start() {
            levelNames = new List<string>();
            levelNames.Add("GameScene");
            levelNames.Add("Level 10");
            levelNames.Add("Ball Tester"); 
            levelNames.Add("Level 8"); 
            levelNames.Add("Level 9"); 
            levelNames.Add("Level 8"); 
            levelNames.Add("Level 9"); 
            levelNames.Add("Level 8"); 
            levelNames.Add("Level 9"); 
            levelNames.Add("Level 8"); 
            levelNames.Add("Level 9"); 
            // levelNames = new string[] {"GameScene", "Ball Tester", "Level 9"};
            // currentState = GameState.Warmup; 
            Debug.Log("destroyar inte");
            DontDestroyOnLoad(gameObject);
            var scene = SceneManager.GetActiveScene();
            var sceneMode = scene.isLoaded ? LoadSceneMode.Additive : LoadSceneMode.Single;
            OnSceneLoaded(scene, sceneMode);
        }

        private void Update() {
            var sceneName = SceneManager.GetActiveScene().name;
            if (sceneName is "Network" or "LobbyScene" or "ErrorScene") {
                return;
            }

            if (!isServer) {
                // Debug.Log("vi är inte server");
                return;
            }

            CheckStates(currentState);
            CheckHotkeys();
        }


        private void CheckStates(GameState currentStateInput) {
            if (levelManager == null) {
                return; 
            }
            
            var script = levelManager.GetComponent<WaterballLevelManager>(); 
            
            switch (currentStateInput) {
                case GameState.Loading:
                    return;
                case GameState.Warmup:
                    // Debug.Log("vi är i warmup");
                    StartCoroutine(Warmup(script)); 
                    return;
                case GameState.BallSpawning:
                    // Debug.Log("vi är i ball spawning");
                    StartCoroutine(BallSpawning(script));
                    return;
                case GameState.Playing:
                    // Debug.Log("vi är i playing");
                    Playing(script);
                    return;
                case GameState.EndingLevel:
                    // Debug.Log("vi är i ending");
                    StartCoroutine(Ending(script));
                    return;
                default:
                    return;
            }
        }


        [Server]
        private IEnumerator Warmup(WaterballLevelManager script) {
            if (script.isPlayingGetReady) {
                yield break;
            }

            Debug.Log("startar get ready coroutine");
            warmupCoroutine = StartCoroutine(script.StartGetReadyBannerCoroutine());
            yield return warmupCoroutine;
            warmupCoroutine = null; 
            Debug.Log("get ready coroutine klar");
            currentState = GameState.BallSpawning; 
        }

        
        [Server]
        private IEnumerator BallSpawning(WaterballLevelManager script) {
            if (script.isBallSpawning) {
                yield break; 
            }

            Debug.Log("startar ball spawning coroutine");
            ballSpawningCoroutine = StartCoroutine(script.SpawnBallsCoroutine());
            yield return ballSpawningCoroutine;
            ballSpawningCoroutine = null; 
            Debug.Log("ball spawning coroutine klar");
            currentState = GameState.Playing; 
        }
        
        
        [Server]
        private void Playing(WaterballLevelManager script) {
            if (script.IsWinConditionMet()) {
                if (script.isWon) {
                    currentState = GameState.EndingLevel;
                    return;
                }

                if (!script.isPlayingCountdown) {
                    StartCoroutine(script.StartCountdownBannerCoroutine());
                }
            }
            else {

                if (script.isPlayingCountdown) {
                    script.StopCountdownBanner();
                }
            }
        }
        
        
        [Server]
        private IEnumerator Ending(WaterballLevelManager script) {
            if (script.isPlayingEnding) {
                yield break;
            }

            endingCoroutine = StartCoroutine(script.StartEndingBanner());
            yield return endingCoroutine;
            endingCoroutine = null; 
            LoadNextLevel();
            currentState = GameState.Loading;
        }
        
        
        private void StopAllGameCoroutines() {
            if (warmupCoroutine != null) {
                StopCoroutine(warmupCoroutine);
                warmupCoroutine = null;
            }

            if (ballSpawningCoroutine != null) {
                StopCoroutine(ballSpawningCoroutine);
                ballSpawningCoroutine = null;
            }

            if (endingCoroutine != null) {
                StopCoroutine(endingCoroutine);
                endingCoroutine = null;
            }
        }
        
        

        [Server]
        private void CheckHotkeys() {
            if (levelManager == null) {
                return; 
            }
            var script = levelManager.GetComponent<WaterballLevelManager>();

            if (Input.GetKeyDown(KeyCode.K)) {
                LoadNextLevel();
            }

            if (Input.GetKeyDown(KeyCode.O)) {
                StartCoroutine(script.StartGetReadyBannerCoroutine()); 
            }

            if (Input.GetKeyDown(KeyCode.P)) {
                StartCoroutine(script.SpawnBallsCoroutine()); 
            }

            if (Input.GetKeyDown(KeyCode.U)) {
                script.MoveBallsToMiddle();
            }

            if (Input.GetKeyDown(KeyCode.H)) {
                StartCoroutine(script.StartCountdownBannerCoroutine());
            }

            if (Input.GetKeyDown(KeyCode.N)) {
                script.StopCountdownBanner();
            }

            if (Input.GetKeyDown(KeyCode.Y)) {
                StartCoroutine(script.StartEndingBanner());
            }
        }

        [Server]
        private void LoadNextLevel() {
            currentState = GameState.Loading; 
            currentLevel++;
            if (currentLevel >= levelNames.Count) {
                currentLevel = 0;
            }

            // Debug.Log(currentLevel);
            // Debug.Log(levelNames);
            // Debug.Log(levelNames[currentLevel]);
            StopAllGameCoroutines(); 
            var networkManager = GameObject.Find("Advanced Network Configuration");
            var script = networkManager.GetComponent<WaterballNetworkManager>();
            script.ServerChangeScene(levelNames[currentLevel]);
            
            // transform.parent.GetComponent<GameObject>().GetComponent<WaterballNetworkManager>().ServerChangeScene(levelNames[currentLevel]);
        }


        // scene loaded hook

        [Server]
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            Debug.Log("New scene loaded: " + scene.name);
            if (scene.name is "Network" or "LobbyScene" or "ErrorScene") {
                return;
            }

            StartCoroutine(OnSceneLoadedDelayed(1f));

            if (scene.name is "GameScene") {
                return; 
            }
            WaterballAudioManager.Instance.isMuted = false; 
        }

        [Server]
        private IEnumerator OnSceneLoadedDelayed(float waitingTime) {
            yield return new WaitForSeconds(waitingTime);
            levelManager = GameObject.Find("LevelManager");
      
            SubToEveryoneReady(); 
            currentState = GameState.Warmup;
            // isLoading = false;
        }


        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }


        private void SubToEveryoneReady() {
            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players) {
                player.GetComponent<PuzzleBehaviour>().everyoneReadyEvent += ReactOnEveryoneReady; 
            }
        }
            
            
        private void ReactOnEveryoneReady() {
            var sceneName = SceneManager.GetActiveScene().name;
            if (sceneName is not "GameScene") {
                return; 
            }
            
            LoadNextLevel();
        }
        
        
    }
}




// [Server]
// private IEnumerator BallSpawningCoroutine() {
//     isSpawningBalls = true;
//     var spawnerScript = ballSpawner.GetComponent<WaterballBallSpawner>();
//     var spawningTime = spawnerScript.oneWayColliderTimeActive;
//     StartCoroutine(spawnerScript.SpawnBalls());
//     ballsTotal += spawnerScript.numberOfBalls;
//     yield return new WaitForSeconds(spawningTime);
//     currentState = GameState.Playing;
//     isSpawningBalls = false;
// }


// [Server]
// private IEnumerator ShowGetReadyBanner() {
//     isPlayingBanner = true;
//     GameObject bannerInstance = Instantiate(bannerPrefab);
//     NetworkServer.Spawn(bannerInstance);
//     // Debug.Log("spawning a banner");
//     float seconds = bannerInstance.GetComponent<WaterballBanner>().totalDisplayTime;
//     yield return new WaitForSeconds(seconds);
//     isPlayingBanner = false;
//
//     currentState = GameState.BallSpawning;
// }


// [Server]
// private void Playing() {
//     if (ballsTotal - ballsInGoal <= 0) {
//         currentState = GameState.EndingLevel;
//     }
// }


// [Server]
// private IEnumerator EndingLevel() {
//     isEndingLevel = true;
//     GameObject endingInstance = Instantiate(endingPrefab);
//     NetworkServer.Spawn(endingInstance);
//     // Debug.Log("spawning an ending instance prefab");
//     float seconds = endingInstance.GetComponent<WaterballEnding>().totalDisplayTime;
//     yield return new WaitForSeconds(seconds);
//     isEndingLevel = false;
//     ballsInGoal = 0;
//     ballsTotal = 0;
//     currentState = GameState.Loading;
// }



// switch (currentStateInput) {
//     case GameState.Loading:
//         if (!isLoading) {
//             break;
//         }
//
//         LoadNextLevel();
//         break;
//
//     case GameState.Warmup:
//
//         if (!isPlayingBanner) {
//             StartCoroutine(ShowGetReadyBanner());
//         }
//
//         Debug.Log("warmup");
//         break;
//     case GameState.BallSpawning:
//
//         if (!isSpawningBalls) {
//             StartCoroutine(BallSpawningCoroutine());
//         }
//
//
//         Debug.Log("ball spawning");
//
//
//         // currentState = GameState.Playing;
//         break;
//     case GameState.Playing:
//         Debug.Log("playing");
//         Playing();
//         break;
//     case GameState.EndingLevel:
//         if (!isEndingLevel) {
//             StartCoroutine(EndingLevel());
//         }
//
//         Debug.Log("ending level");
//         break;
//     default:
//         Debug.Log("inget state");
//         break;
// }



// [Server]
// private void BallEnteredGoal() {
//     ballsInGoal++;
//     // Debug.Log($"ball entered! ballsLeft: {ballsTotal - ballsTotal}");
//     goal.GetComponent<NewGoal>().setBallRatio(calculateBallRatio(ballsInGoal, ballsTotal));
// }
//
// [Server]
// private void BallExitedGoal() {
//     ballsInGoal--;
//     // Debug.Log($"ball exited! ballsLeft: {ballsTotal - ballsInGoal}");
//     goal.GetComponent<NewGoal>().setBallRatio(calculateBallRatio(ballsInGoal, ballsTotal));
// }


// public GameObject bannerPrefab;
// public GameObject ballPrefab;
// public GameObject endingPrefab;
// public GameObject countdownBannerPrefab;
// private int ballsInGoal = 0;
// private int ballsTotal = 0;
// private GameObject ballSpawner;
// private Coroutine countdownCoroutine;
// private WaterballCountdownBanner countdownBannerComponent;
// private GameObject goal;


// private void MoveBallsToMiddle() {
//
//     var objects = GameObject.FindGameObjectsWithTag("Ball");
//
//
//     for (var i = 0; i < objects.Length; i++) {
//
//         var extraHeight = new Vector3(0, i + 2, 0);
//         objects[i].transform.position = Vector3.zero + extraHeight;
//     }
// }


// private float calculateBallRatio(int ballsInGoal, int ballsTotal) {
//     if (ballsTotal == 0) {
//         return 1f;
//     }
//
//     if (ballsInGoal >= ballsTotal) {
//         return 1f;
//     }
//
//     return (float) ballsInGoal / ballsTotal;
// }


// currentLevel++;
// if (currentLevel >= levelNames.Length) {
//     currentLevel = 0;
// }
//
// Debug.Log(currentLevel);
// Debug.Log(levelNames);
// Debug.Log(levelNames[currentLevel]);
// GetComponent<CITENetworkManager>().ServerChangeScene(levelNames[currentLevel]);
// bannerInstance = null;


// if (SceneManager.GetActiveScene().name == "Network") {
//     Debug.Log("vi är i network");
//     return;
// }
//
// if (SceneManager.GetActiveScene().name == "LobbyScene") {
//     Debug.Log("vi är i lobby");
//     return;
// }
//
// if (SceneManager.GetActiveScene().name == "ErrorScene") {
//     Debug.Log("vi är i error");
//     return;
// }


// ballSpawner = GameObject.Find("BallSpawner");
// ballsTotal = 0;
// ballsInGoal = 0;
// if (ballSpawner == null) {
//     throw new InvalidOperationException("No BallSpawner found");
// }
//
// goal = GameObject.Find("NewGoal");
// if (goal is null) {
//     throw new InvalidOperationException("No Goal found found");
// }
//
// var goalScript = goal.GetComponent<NewGoal>();
// goalScript.BallEnteredGoalEvent += BallEnteredGoal;
// goalScript.BallExitedGoalEvent += BallExitedGoal;
// // Debug.Log("goalscript!!:" + goalScript);


//
// // test methods below
//
// [Server]
// public void SpawnBall() {
//     GameObject ballInstance = Instantiate(ballPrefab, new Vector3(0, 2, 0), Quaternion.identity);
//     NetworkServer.Spawn(ballInstance);
//     Debug.Log("spawning a ball");
// }
//
// [Server]
// public void SpawnBanner() {
//     GameObject bannerInstance = Instantiate(bannerPrefab);
//     NetworkServer.Spawn(bannerInstance);
//     Debug.Log("spawning a banner");
// }
//
// [Server]
// public void SpawnBalls() {
//     GameObject ballSpawner = GameObject.Find("BallSpawner");
//     if (ballSpawner is null) {
//         return;
//     }
//
//     var spawnerScript = ballSpawner.GetComponent<WaterballBallSpawner>();
//     StartCoroutine(spawnerScript.SpawnBalls());
//     ballsTotal += spawnerScript.numberOfBalls;
// }

// [Server]
// private void SpawnCountdownBanner() {
//     var countdown = Instantiate(countdownBannerPrefab);
//     NetworkServer.Spawn(countdown);
//     Debug.Log("hello");
//     countdownBannerComponent = countdown.GetComponent<WaterballCountdownBanner>();
//     // countdownBannerComponent.StartTimer(); 
// }
//
//
// [Server]
// private void StartCountdownTimer() {
//     if (countdownBannerComponent == null) {
//         SpawnCountdownBanner();
//     }
//     
//     countdownBannerComponent.StartTimerClientRpc();
// }
//
//
// [Server]
// private void StopCountdownTimer() {
//     if (countdownBannerComponent == null) {
//         SpawnCountdownBanner();
//     }
//     
//     countdownBannerComponent.StopTimerClientRpc();
// }
//


// [Server]
// public void SpawnCountdown() {
//     GameObject countdownInstance = Instantiate(countdownBanner);
//     countdownBannerComponent = countdownInstance.GetComponent<WaterballCountdownBanner>(); 
//     NetworkServer.Spawn(countdownInstance);
//     Debug.Log("spawning a countdown");
// }
//
//
// [ClientRpc]
// public void StartCountdownRpc() {
//     if (countdownBannerComponent == null) {
//         SpawnCountdown();
//     } 
//     countdownCoroutine = StartCoroutine(countdownBannerComponent.StartTimer());
// }
//
// [ClientRpc]
// public void StopCountdownRpc() {
//     if (countdownBannerComponent != null && countdownCoroutine != null) {
//         countdownBannerComponent.StopTimer();
//         // StopCoroutine(countdownCoroutine);
//         // countdownCoroutine = null;
//     }
// }


// [Server]
// public void SpawnCountdown()
// {
//     RpcSpawnCountdown();
// }
//
// [ClientRpc]
// public void RpcSpawnCountdown()
// {
//     GameObject countdownInstance = Instantiate(countdownBannerPrefab);
//     countdownBannerComponent = countdownInstance.GetComponent<WaterballCountdownBanner>();
//     Debug.Log("spawning a countdown");
// }
//
// [ClientRpc]
// public void RpcStartCountdown()
// {
//     // if (countdownBannerComponent == null)
//     // {
//         RpcSpawnCountdown();
//     // }
//     countdownCoroutine = StartCoroutine(countdownBannerComponent.StartTimer());
// }
//
// [ClientRpc]
// public void RpcStopCountdown()
// {
//     if (countdownBannerComponent != null && countdownCoroutine != null)
//     {
//         countdownBannerComponent.StopTimer();
//         // StopCoroutine(countdownCoroutine);
//         // countdownCoroutine = null;
//     }
// }


// [Server]
// private IEnumerator SpawnBalls() {
//     WaterballLevelConfig levelConfig = FindObjectOfType<WaterballLevelConfig>();
//     int numberOfBalls = levelConfig.numberOfBalls;
//     float minBallSpeed = minBallSpawningSpeed; 
//
//     Vector3 position = new Vector3(0, 4, 0);  
//     while (numberOfBalls > 0) {
//         Quaternion ballDirection = GetRandomBallDirection(); 
//         GameObject ballInstance = Instantiate(ballPrefab, position, ballDirection);
//         Vector3 velocity = GetRandomBallVelocity(minBallSpeed, ballInstance);
//         ballInstance.GetComponent<Rigidbody>().velocity = velocity; 
//         NetworkServer.Spawn(ballInstance);
//         position = GetNextBallSpawnPosition(position);
//         numberOfBalls--; 
//     }
//     yield return null; 
// }
//
//
// private Vector3 GetNextBallSpawnPosition(Vector3 lastPosition) {
//     return lastPosition + Vector3.up; 
// }
//
// private Quaternion GetRandomBallDirection() {
//     float multiplier = (float) rd.NextDouble();
//     Quaternion rotationY = Quaternion.Euler(0, 360 * multiplier, 0);
//     Quaternion rotationX = Quaternion.Euler(0, 0, 0);
//     Quaternion finalRotation = rotationY * rotationX; 
//     return finalRotation;
// }
//
// private Vector3 GetRandomBallVelocity(float minBaseSpeed, GameObject ballInstance) {
//     float forwardRoll = (float) rd.NextDouble();
//     Vector3 velocity = ballInstance.transform.forward * (forwardRoll + 1) * minBaseSpeed;
//     Debug.Log(velocity);
//
//     return velocity;
// }

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


// [Server]
// private IEnumerator DoBallSpawningPhase() {

// }


// forward * 


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