using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PuzzleBehaviour : NetworkBehaviour {
    public int puzzleSegment;
    private bool wasReady;

    // Start is called before the first frame update
    void Start(){
        wasReady = false;
    }

    // Tell all the views in the scene about us
    public override void OnStartLocalPlayer(){
        puzzleSegment = gameObject.GetComponent<GameBehaviour>().playerID;
        foreach (PuzzleView view in FindObjectsOfType<PuzzleView>()){
            view.OnPuzzleStarted(this);
        }
    }

    public void setReadyness(bool isReady){
        if (isReady && !wasReady){
            wasReady = true;
            Debug.Log("Signal: Ready");
            CmdSetReadyness(wasReady);
        } else if (wasReady & !isReady){
            wasReady = false;
            Debug.Log("Signal: Not ready anymore");
            CmdSetReadyness(wasReady);
        }
    }

    // Tell the server if we are ready to proceed to hide the puzzle or not
    [Command] void CmdSetReadyness(bool iAmReady){
        wasReady = iAmReady; // Set readyness on server

        // Check everyone
        bool everyoneReady = true;
        foreach (PuzzleBehaviour participant in FindObjectsOfType<PuzzleBehaviour>()){
            if (!participant.wasReady) everyoneReady = false;
        }

        // If all are ready, inform the clients
        if (everyoneReady){
            foreach (PuzzleBehaviour participant in FindObjectsOfType<PuzzleBehaviour>()){
                participant.RpcOnEveryoneReady();
            }
        }
    }

    /**
     * Called on clients when enough players have signalled that they are ready to start
     */
    [ClientRpc] public void RpcOnEveryoneReady() {
        // Tell the view that it is over
        Debug.Log("Everybody is ready!");
        foreach (PuzzleView view in FindObjectsOfType<PuzzleView>()){
            view.OnPuzzleEnded(this);          
        }
        this.enabled = false; // Shutdown ourselves
    }
}
