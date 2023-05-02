using System;
using Mirror;
using UnityEngine;

public interface IGoal {
     void SetBallRatio(float newRatio);
     event Action BallEnteredGoalEvent;
     event Action BallExitedGoalEvent;
}