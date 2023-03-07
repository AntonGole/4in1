using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleView : MonoBehaviour {
    private PuzzleBehaviour puzzle;
    public Text playerText;
    public Image[] puzzlePieces;

    // Called when the puzzle logic has loaded
    public void OnPuzzleStarted(PuzzleBehaviour puzzleBehaviour) {
        puzzle = puzzleBehaviour;
        Debug.Log("Wanted to puzzle segment " + puzzle.puzzleSegment);
        puzzlePieces[puzzle.puzzleSegment].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update(){
        // Ready if something is touching the screen
        if (puzzle != null) {
            puzzle.setReadyness(Input.touchCount > 0 || Input.GetMouseButton(0));
        }
    }

    /**
     * Called when everyone is ready
     */
    public void OnPuzzleEnded(PuzzleBehaviour unused){
        // Hide the puzzle view
        gameObject.SetActive(false);
    }
}
