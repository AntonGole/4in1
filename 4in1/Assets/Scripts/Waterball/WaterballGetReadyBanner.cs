using System;
using UnityEngine;

//using System;
using System.Collections;
using Mirror;
//using UnityEngine;
using UnityEngine.UI;


public class WaterballGetReadyBanner : NetworkBehaviour {
    public GameObject canvas;
    public GameObject circularBanner;
    // public GameObject countdownNumbers;

    public GameObject[] numberHolders;
    public Sprite[] numberSprites;
    public float circularBannerRotationSpeed = 30f;
    // public float totalDisplayTime = 4f;

    public bool isRunning = false;

    public float duration = 4f;
    // public float initialDelay = 1f;


    private float tStart;
    private float tEnd;


    private void Start() {
    }

    private void Update() {
        if (!circularBanner) {
            return;
        }

        RotateBanner();
        UpdateNumbers();
    }

    [ClientRpc]
    public void StartBannerClientRpc() {
        StartCoroutine(StartBannerCoroutine());
    }

    
    private IEnumerator StartBannerCoroutine() {
        isRunning = true;
        tStart = Time.time;
        tEnd = tStart + duration;
        canvas.SetActive(true);
        // StartCoroutine(ShowCircularBanner(totalDisplayTime));
        // StartCoroutine(ShowCountdown(totalDisplayTime / 4));

        while (isRunning && Time.time < tEnd) {
            yield return null;
        }

        canvas.SetActive(false);
    }


    [ClientRpc]
    public void StopBannerClientRpc() {
        isRunning = false;
    }


    private void RotateBanner() {
        circularBanner.transform.Rotate(new Vector3(0, 0, -circularBannerRotationSpeed * Time.deltaTime));
    }

    private void UpdateNumbers() {
        foreach (var numberHolder in numberHolders) {
            var timeRatio = (Time.time - tStart) / (tEnd - tStart);
            var index = DetermineCurrentNumberIndex(timeRatio);
            if (index == -1) {
                numberHolder.SetActive(false);
            }
            else {
                numberHolder.SetActive(true);
                Image numberImage = numberHolder.transform.GetChild(0).GetComponent<Image>();
                numberImage.sprite = numberSprites[index];
            }
        }
    }


    private int DetermineCurrentNumberIndex(float timeRatio) {
        switch (timeRatio) {
            case <= 0.25f:
                return -1;
            case <= 0.5f:
                return 0;
            case <= 0.75f:
                return 1;
            case <= 1f:
                return 2;
            default:
                return -1;
        }
    }
}


// private IEnumerator ShowCountdown(float displayTime) {
//     countdownNumbers.SetActive(false);
//     yield return new WaitForSeconds(displayTime);
//     countdownNumbers.SetActive(true);
//     foreach (var numberSprite in numberSprites) {
//         foreach (var numberHolder in numberHolders) {
//             Image numberImage = numberHolder.transform.GetChild(0).GetComponent<Image>();
//             numberImage.sprite = numberSprite;
//         }
//
//         yield return new WaitForSeconds(displayTime);
//     }
//
//     countdownNumbers.SetActive(false);
// }


// private IEnumerator ShowCircularBanner(float displayTime) {
// circularBanner.SetActive(true);
// yield return new WaitForSeconds(displayTime); 
// circularBanner.SetActive(false);
// }