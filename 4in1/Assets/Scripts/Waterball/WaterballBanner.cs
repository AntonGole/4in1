using System;
using UnityEngine;

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; 


namespace DefaultNamespace {
    public class WaterballBanner : MonoBehaviour {


        public GameObject circularBanner;
        public GameObject countdownNumbers; 
        
        public GameObject[] numberHolders;
        public Sprite[] numberSprites;
        public float circularBannerRotationSpeed = 30f;
        public float totalDisplayTime = 4f; 


        private void Start() {
            StartCoroutine(ShowCircularBanner(totalDisplayTime));
            StartCoroutine(ShowCountdown(totalDisplayTime / 4));
        }
        
        private void Update() {
            if (!circularBanner) {
                return; 
            }
            circularBanner.transform.Rotate(new Vector3(0, 0, -circularBannerRotationSpeed * Time.deltaTime));
        }

        private IEnumerator ShowCircularBanner(float displayTime) {
            circularBanner.SetActive(true);
            yield return new WaitForSeconds(displayTime); 
            circularBanner.SetActive(false);
        }
        
        
        private IEnumerator ShowCountdown(float displayTime) {
            countdownNumbers.SetActive(false);
            yield return new WaitForSeconds(displayTime);
            countdownNumbers.SetActive(true);
            foreach (var numberSprite in numberSprites) {
                foreach (var numberHolder in numberHolders) {
                    Image numberImage = numberHolder.transform.GetChild(0).GetComponent<Image>();
                    numberImage.sprite = numberSprite;
                }
                yield return new WaitForSeconds(displayTime);
            }
            countdownNumbers.SetActive(false);
        }
        
    }
}