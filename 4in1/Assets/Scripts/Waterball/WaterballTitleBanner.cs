using System;
using UnityEngine;

//using System;
using System.Collections;
using Mirror;
//using UnityEngine;
using UnityEngine.UI;


public class WaterballTitleBanner : NetworkBehaviour {
    public GameObject canvas;
    public GameObject circularBanner;

    public float circularBannerRotationSpeed = 30f;

    private float lastRotation;
    private float lerpSpeed = 10f; 
    
    [SyncVar]
    public float syncedRotation;

    private void Start() {
        if (isServer) {
            // If this is the server, set the initial rotation value
            syncedRotation = transform.rotation.eulerAngles.z;
        }

        lastRotation = syncedRotation;
    }

    private void Update() {
        if (!circularBanner) {
            return;
        }

        if (isServer) {
            syncedRotation += circularBannerRotationSpeed * Time.deltaTime;
            syncedRotation = syncedRotation % 360f; 
        }
        else {
            lastRotation = Mathf.Lerp(lastRotation, syncedRotation, Time.deltaTime * lerpSpeed);
        }

        RotateBanner();
    }
    
    
    
  




    private void RotateBanner() {
        circularBanner.transform.localRotation = Quaternion.Euler(0, 0, isServer ? syncedRotation : lastRotation);
        // circularBanner.transform.Rotate(new Vector3(0, 0, -circularBannerRotationSpeed * Time.deltaTime));
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