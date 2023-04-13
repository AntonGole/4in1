using System;
using System.Collections;
using Mirror;
using UnityEngine.UI;
using UnityEngine;

public class WaterballCountdownBanner : MonoBehaviour {
    public GameObject bannerHolder;
    public Image[] filleds;
    public float duration = 3f;

    public float filledGrade = 0;
    private float tStart;
    private float tEnd;
    private float fillTime;
    private float fillRate;
    private float totalFilledPercentage = 0.25f;

    private bool stopTimer = false; 
    

    private void Start() {
        // Debug.Log("hello!!");
        fillTime = (float) 3 / 5 * duration;
        fillRate = totalFilledPercentage / fillTime;
    }
    
    
    


    private void Update() {
        var currentTime = Time.time;
        var rate = DetermineFillAmountRateOfChange(currentTime);
        AdjustFilled(filledGrade + rate * Time.deltaTime);
    }


    [ClientRpc]
    public IEnumerator StartTimer() {
        stopTimer = false; 
        filledGrade = 0;
        tStart = Time.time;
        tEnd = tStart + duration;
        bannerHolder.SetActive(true);
        // StartCoroutine(ShowCountdown(duration));

        while (!stopTimer && Time.time < tEnd) {
            yield return null; 
        }
        bannerHolder.SetActive(false);
        // yield return new WaitForSeconds(duration);
        
        
    }
    
    [ClientRpc]
    public void StopTimer() {

        stopTimer = true; 
        // filledGrade = 0;
        // bannerHolder.SetActive(false);
    }
    

    private float DetermineFillAmountRateOfChange(float currentTime) {
        var fraction = (currentTime - tStart) / (tEnd - tStart);
        switch (fraction) {
            case <= 0.2f:
                return fillRate;
            case <= 0.4f:
                return 0;
            case <= 0.6f:
                return fillRate;
            case <= 0.8f:
                return 0;
            case <= 1.0f:
                return fillRate;
            default:
                return 0;
        }
    }


    private void AdjustFilled(float filledGradeInput) {
        foreach (var filled in filleds) {
            filled.fillAmount = filledGradeInput;
            filledGrade = filledGradeInput; 
        }
    }


    // private IEnumerator ShowCountdown(float duration) {
        // 
        
        // 
    // }


}