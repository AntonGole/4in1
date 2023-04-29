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



    private void Start() {

    }

    private void Update() {
        if (!circularBanner) {
            return;
        }

        RotateBanner();
    }
    
    
    
  




    private void RotateBanner() {
        circularBanner.transform.Rotate(new Vector3(0, 0, -circularBannerRotationSpeed * Time.deltaTime));
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