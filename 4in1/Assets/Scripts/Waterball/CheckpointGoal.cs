using System.Collections.Generic;
using UnityEngine;

public class CheckpointGoal : NewGoal
{
    [SerializeField] private GameObject[] checkpoints; 
    private Queue<GameObject> checkpointQueue;

    private void Start()
    {
        base.Start();
        InitializeCheckpoints();
    }

    private void InitializeCheckpoints()
    {
        checkpointQueue = new Queue<GameObject>(checkpoints);
        ResetCheckpoints();
    }

    private void ResetCheckpoints()
    {
        foreach (GameObject checkpoint in checkpoints)
        {
            checkpoint.SetActive(false);
        }

        if (checkpointQueue.Count > 0)
        {
            checkpointQueue.Peek().SetActive(true);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball"))
        {
            return;
        }

        // ball collides with active checkpoint
        if (other.transform.IsChildOf(checkpointQueue.Peek().transform))
        {
            checkpointQueue.Dequeue().SetActive(false);

            if (checkpointQueue.Count > 0)
            {
                checkpointQueue.Peek().SetActive(true);
            }
            else
            {
                // all checkpoints passed
                BallEnteredGoalEvent?.Invoke();
            }
        }
        else
        {
            // ball activates wrong checkpoint, reset the checkpoints
            foreach (GameObject checkpoint in checkpoints)
            {
                if (other.transform.IsChildOf(checkpoint.transform))
                {
                    InitializeCheckpoints();
                    break;
                }
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        // empty as the ball exiting is not relevant for this goal type
    }
}