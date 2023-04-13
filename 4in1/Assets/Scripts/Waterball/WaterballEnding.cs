using System;
using UnityEngine;

//using System;
using System.Collections;
//using UnityEngine;
using UnityEngine.UI;


namespace DefaultNamespace
{
    public class WaterballEnding : MonoBehaviour {

        public ParticleSystem confettiPrefab; 
        public GameObject[] textHolders;
        public GameObject grouper;
        public Sprite textSprite;
        public float totalDisplayTime = 4f;


        private void Start()
        {
            PlayConfetti();
            StartCoroutine(ShowEndingText(totalDisplayTime));
        }

        private void Update()
        {
            return;
        }

        private void PlayConfetti() {
            // var parent = transform; 
            var position = new Vector3(0, 2, 0);
            var direction = Quaternion.LookRotation(Vector3.up); 
            var confetti = Instantiate(confettiPrefab, position, direction);
            confetti.Play();
            Destroy(confetti.gameObject, confetti.main.startLifetime.constantMax);
        }

        private IEnumerator ShowEndingText(float displayTime)
        {
            grouper.SetActive(true);
            foreach (var textHolder in textHolders)
            {
                Image textImage = textHolder.transform.GetChild(0).GetComponent<Image>();
                textImage.sprite = textSprite;
            }
            yield return new WaitForSeconds(displayTime);
            grouper.SetActive(false);
        }
    }
}