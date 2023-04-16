using System;
using UnityEngine;


public class WaterballGoalOld : MonoBehaviour {
    public event Action BallEnteredGoalEvent;
    public event Action BallExitedGoalEvent;

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