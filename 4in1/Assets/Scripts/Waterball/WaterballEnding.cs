using System;
using UnityEngine;

//using System;
using System.Collections;
using System.Data.SqlTypes;
using Mirror;
using UnityEngine.Experimental.Rendering;
//using UnityEngine;
using UnityEngine.UI;


public class WaterballEnding : NetworkBehaviour {
    public GameObject canvas;

    public ParticleSystem confettiPrefab;

    // public GameObject[] textHolders;
    // public GameObject grouper;
    // public Sprite textSprite;
    // public float totalDisplayTime = 4f;
    public float duration = 4;

    private bool isPlaying = false;

    private float tStart;
    private float tEnd;

    private void Start() {
        // PlayConfetti();
        // StartCoroutine(ShowEndingText(totalDisplayTime));
    }

    private void Update() {
        return;
    }


    [ClientRpc]
    public void StartBannerClientRpc() {
        StartCoroutine(StartBannerCoroutine());
    }
    
    
    private IEnumerator StartBannerCoroutine() {
        isPlaying = true;
        tStart = Time.time;
        tEnd = tStart + duration;
        canvas.SetActive(true);
        PlayConfetti();
        while (isPlaying && Time.time < tEnd) {
            yield return null;
        }

        canvas.SetActive(false);
    }


    [ClientRpc]
    public void StopBannerClientRpc() {
        isPlaying = false;
    }


    private void PlayConfetti() {
        var position = new Vector3(0, 2, 0);
        var direction = Quaternion.LookRotation(Vector3.up);
        var confetti = Instantiate(confettiPrefab, position, direction);
        confetti.Play();
        Destroy(confetti.gameObject, confetti.main.startLifetime.constantMax);
    }
}


// private IEnumerator ShowEndingText(float displayTime)
// {
//     grouper.SetActive(true);
//     foreach (var textHolder in textHolders)
//     {
//         Image textImage = textHolder.transform.GetChild(0).GetComponent<Image>();
//         textImage.sprite = textSprite;
//     }
//     yield return new WaitForSeconds(displayTime);
//     grouper.SetActive(false);
// }