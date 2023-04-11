using System;
using UnityEngine;

public class NewGoal : MonoBehaviour {
    
    
    public float maxBalls = 10; // The total number of balls to reach a full blend
    public float currentBalls = 5; /* the current number of balls in the goal area */
    
    
    private Material goalMaterial; /* the reference to the CustomCheckerboardBlending material */


    private void Start() {
        goalMaterial = transform.GetChild(0).GetComponent<Renderer>().material;
        Debug.Log(goalMaterial);
    }

    private void Update() {

        float blendFactor = currentBalls / maxBalls; 
        goalMaterial.SetFloat("_Blend", blendFactor);
        


        // private float blendFactor = currentBalls / maxBalls;
    }

    
    
}