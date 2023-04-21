using System;
using Mirror;
using UnityEngine;

public class WaterballGoal : NetworkBehaviour {
    // public float maxBalls = 5; // The total number of balls to reach a full blend
    // public float currentBalls = 5; /* the current number of balls in the goal area */

    public Vector2 defaultSize = new Vector2(1, 1);
    public GameObject[] goalSurfaces;

    // private Material goalMaterial; /* the reference to the CustomCheckerboardBlending material */

    [SyncVar(hook = nameof(OnBallRatioChanged))]
    public float ballRatio = 1;



    public event Action BallEnteredGoalEvent;
    public event Action BallExitedGoalEvent;

    public void SetBallRatio(float newRatio) {
        ballRatio = newRatio; 
    }


    private void OnBallRatioChanged(float oldRatio, float newRatio) {
        Debug.Log("got a color update: " + newRatio);
        foreach (var surface in goalSurfaces) {
            var goalMaterial = surface.GetComponent<Renderer>().material; 
            goalMaterial.SetFloat("_Blend", ballRatio);
        }
    }


    private void Start() {
        //     goalMaterial = transform.GetChild(0).GetComponent<Renderer>().material;
        //     Debug.Log(goalMaterial);
        AdjustTiling();
        OnBallRatioChanged(1, 1);
    }
    //
    // private void Update() {
    //     float blendFactor = currentBalls / maxBalls;
    //     goalMaterial.SetFloat("_Blend", blendFactor);
    //
    //     // private float blendFactor = currentBalls / maxBalls;
    // }


    private void AdjustTiling() {
        foreach (var surface in goalSurfaces) {
            MeshRenderer meshRenderer = surface.GetComponent<MeshRenderer>();
            if (meshRenderer != null && meshRenderer.sharedMaterial != null) {
                Vector3 objectSize = meshRenderer.bounds.size;
                Vector2 tiling = new Vector2(objectSize.x / defaultSize.x, objectSize.z / defaultSize.y);
                meshRenderer.sharedMaterial.mainTextureScale = tiling;
            }
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Ball")) {
            return;
        }

        BallEnteredGoalEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Ball")) {
            return;
        }

        BallExitedGoalEvent?.Invoke();
    }
    
    
        
        
        
}