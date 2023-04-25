using System;
using Mirror;
using UnityEngine;

public class WaterballGoal2 : NetworkBehaviour
{
    // public float maxBalls = 5; // The total number of balls to reach a full blend
    // public float currentBalls = 5; /* the current number of balls in the goal area */

    public Vector2 defaultSize = new Vector2(1, 1);
    public GameObject[] goalSurfaces;

    // private Material goalMaterial; /* the reference to the CustomCheckerboardBlending material */




    public event Action BallEnteredGoalEvent;


    private void Start()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball"))
        {
            return;
        }

        BallEnteredGoalEvent?.Invoke();
    }


}