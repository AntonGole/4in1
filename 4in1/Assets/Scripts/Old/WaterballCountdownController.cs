using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; 


    public class WaterballCountdownController : MonoBehaviour {


        public GameObject[] numberHolders;
        public Sprite[] numberSprites;


        public float displayTime = 1f; 
        
        
        private void Start() {
            StartCoroutine(ShowCountdown()); 


        }


        private IEnumerator ShowCountdown() {

            foreach (var numberSprite in numberSprites) {
                foreach (var numberHolder in numberHolders) {
                    Image numberImage = numberHolder.transform.GetChild(0).GetComponent<Image>();
                    numberImage.sprite = numberSprite;
                    // RectTransform rt = numberImage.GetComponent<RectTransform>();
                    // rt.localPosition = Vector3.zero;
                    // rt.localRotation = Quaternion.identity; 
                }
                
                

                // Debug.Log("före vänta");
                yield return new WaitForSeconds(displayTime);

                // Debug.Log("efter vänta");
            }
            


        }
        
    }
