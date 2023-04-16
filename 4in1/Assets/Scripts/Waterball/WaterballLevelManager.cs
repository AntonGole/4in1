using System;
using System.Collections;
using DefaultNamespace;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;


public class WaterballLevelManager : NetworkBehaviour {
    public GameObject getReadyBannerPrefab;
    public GameObject countdownBannerPrefab;
    public GameObject endingBannerPrefab;
    public GameObject ballPrefab;

    public GameObject ballSpawner;
    public GameObject goal;
    private WaterballCountdownBanner countdownBannerComponent;
    private WaterballGetReadyBanner _getReadyGetReadyBannerComponent;
    private WaterballEnding endingBannerComponent;

    private int ballsInGoal = 0;
    private int ballsTotal = 0;

    public bool isPlayingGetReady = false;
    public bool isPlayingCountdown = false;
    public bool isPlayingEnding = false;
    public bool isBallSpawning = false;
    public bool isWon = false;
    // public bool countdownAborted = false;
    private int countdownRuns = 0;
    
    
    private float calculateBallRatio(int ballsInGoal, int ballsTotal) {
        if (ballsTotal == 0) {
            return 1f;
        }

        if (ballsInGoal >= ballsTotal) {
            return 1f;
        }

        return (float) ballsInGoal / ballsTotal;
    }



    public bool IsWinConditionMet() {
        if (ballsTotal <= 0) {
            return true; 
        }

        return ballsInGoal >= ballsTotal; 
    }
    

    public void SpawnBalls() {


        
    }

    private void Start() {
        ballsTotal = 0;
        ballsInGoal = 0; 
        var goalScript = goal.GetComponent<NewGoal>();
        goalScript.BallEnteredGoalEvent += BallEnteredGoal;
        goalScript.BallExitedGoalEvent += BallExitedGoal;
    }
    

    private void BallEnteredGoal() {
        Debug.Log($"ballsInGoal: {ballsInGoal}");
        ballsInGoal++; 
        goal.GetComponent<NewGoal>().setBallRatio(calculateBallRatio(ballsInGoal, ballsTotal));
    }


    private void BallExitedGoal() {
        Debug.Log($"ballsInGoal: {ballsInGoal}");
        ballsInGoal--; 
        goal.GetComponent<NewGoal>().setBallRatio(calculateBallRatio(ballsInGoal, ballsTotal));
    }
    


    [Server]
    public void MoveBallsToMiddle() {
        var objects = GameObject.FindGameObjectsWithTag("Ball");
        for (var i = 0; i < objects.Length; i++) {

            var extraHeight = new Vector3(0, i + 2, 0);
            objects[i].transform.position = Vector3.zero + extraHeight;
        }
    }
    
    [Server]
    public IEnumerator SpawnBallsCoroutine() {
        var spawnerScript = ballSpawner.GetComponent<WaterballBallSpawner>();
        // var spawningTime = spawnerScript.oneWayColliderTimeActive;
        ballsTotal += spawnerScript.numberOfBalls;
        isBallSpawning = true; 
        yield return StartCoroutine(spawnerScript.SpawnBalls());
        isBallSpawning = false; 
    }
    
    [Server]
    private void SpawnGetReadyBanner() {
        var getReadyBanner = Instantiate(getReadyBannerPrefab); 
        NetworkServer.Spawn(getReadyBanner);
        _getReadyGetReadyBannerComponent = getReadyBanner.GetComponent<WaterballGetReadyBanner>(); 
    }

    [Server]
    public IEnumerator StartGetReadyBannerCoroutine() {
        if (_getReadyGetReadyBannerComponent == null) {
            SpawnGetReadyBanner();
        }
        isPlayingGetReady = true;
        var duration = _getReadyGetReadyBannerComponent.duration;
        _getReadyGetReadyBannerComponent.StartBannerClientRpc();
        yield return new WaitForSeconds(duration); 
        isPlayingGetReady = false; 
    }
    
    [Server]
    public void StopGetReadyBanner() {
        if (_getReadyGetReadyBannerComponent == null) {
            SpawnGetReadyBanner();
        }
        
        _getReadyGetReadyBannerComponent.StopBannerClientRpc();
        isPlayingGetReady = false; 
    }

    [Server]
    private void SpawnCountdownBanner() {
        var countdownBanner = Instantiate(countdownBannerPrefab); 
        NetworkServer.Spawn(countdownBanner);
        countdownBannerComponent = countdownBanner.GetComponent<WaterballCountdownBanner>(); 
    }

    [Server]
    public IEnumerator StartCountdownBannerCoroutine() {
        if (countdownBannerComponent == null) {
            SpawnCountdownBanner();
        }
        
        int currentRun = ++countdownRuns; 
        isPlayingCountdown = true;
        var duration = countdownBannerComponent.duration; 
        countdownBannerComponent.StartTimerClientRpc();
        yield return new WaitForSeconds(duration);

        Debug.Log($"current: {currentRun}, count: {countdownRuns}");
        
        if (currentRun == countdownRuns) {
            isWon = true; 
        }
        isPlayingCountdown = false;
    }

    [Server]
    public void StopCountdownBanner() {
        if (countdownBannerComponent == null) {
            SpawnCountdownBanner();
        }

        countdownRuns++; 
        countdownBannerComponent.StopTimerClientRpc();
        isPlayingCountdown = false;
    }
    
    [Server]
    private void SpawnEndingBanner() {
        var endingBanner = Instantiate(endingBannerPrefab); 
        NetworkServer.Spawn(endingBanner);
        endingBannerComponent = endingBanner.GetComponent<WaterballEnding>(); 
    }

    [Server]
    public IEnumerator StartEndingBanner() {
        if (endingBannerComponent == null) {
            SpawnEndingBanner();
        }

        isPlayingEnding = true;
        var duration = endingBannerComponent.duration; 
        endingBannerComponent.StartBannerClientRpc();
        yield return new WaitForSeconds(duration); 
        isPlayingEnding = false; 
    }

    [Server]
    public void StopEndingBanner() {
        if (endingBannerComponent == null) {
            SpawnEndingBanner();
        }

        endingBannerComponent.StopBannerClientRpc();
        isPlayingEnding = false; 
    }
    
    
    

    
    
    
    
    
}






// private IEnumerator BallSpawningCoroutine() {
//     var spawnerScript = ballSpawner.GetComponent<WaterballBallSpawner>();
//     var spawningTime = spawnerScript.oneWayColliderTimeActive;
//     StartCoroutine(spawnerScript.SpawnBalls());
//     ballsTotal += spawnerScript.numberOfBalls;
//     yield return new WaitForSeconds(spawningTime);
// }






