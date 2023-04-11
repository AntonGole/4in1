using System;
using UnityEngine;

//using System;
using System.Collections;
//using UnityEngine;
using UnityEngine.UI;


namespace DefaultNamespace
{
    public class WaterballEnding : MonoBehaviour
    {

        public GameObject[] numberHolders;
        public GameObject countdownNumbers;
        public Sprite textSprite;
        public float totalDisplayTime = 4f;


        private void Start()
        {
            StartCoroutine(ShowEndingText(totalDisplayTime));
        }

        private void Update()
        {
            return;
        }

        private IEnumerator ShowEndingText(float displayTime)
        {
            countdownNumbers.SetActive(true);
            foreach (var numberHolder in numberHolders)
            {
                Image numberImage = numberHolder.transform.GetChild(0).GetComponent<Image>();
                numberImage.sprite = textSprite;
            }
            yield return new WaitForSeconds(displayTime);
            countdownNumbers.SetActive(false);
        }
    }
}