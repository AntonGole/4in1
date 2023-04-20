using System;
using System.Collections;
using Mirror;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class WaterballCountdownBanner : NetworkBehaviour {
    public GameObject canvas;
    public Image[] filleds;
    public float duration = 3f;

    public float filledGrade = 0;
    private float tStart;
    private float tEnd;
    private float fillTime;
    private float fillRate;
    private float totalFilledPercentage = 0.25f;

    private bool stopTimer = false;
    private bool hasPlayedSound = false; 
    

    private void Start() {
        // Debug.Log("hello!!");
        fillTime = (float) 3 / 5 * duration;
        fillRate = totalFilledPercentage / fillTime;
    }
    
    
    


    private void Update() {
        var currentTime = Time.time;
        var rate = DetermineFillAmountRateOfChange(currentTime);
        var previousFilledGrade = filledGrade; 
        AdjustFilled(filledGrade + rate * Time.deltaTime);

        Debug.Log("filledGrade: " + filledGrade);
        
        // if (!isServer) {
        //     return; 
        // }
        //
        // if (filledGrade > previousFilledGrade) {
        //     hasPlayedSound = false; 
        //     return;
        // }
        //
        // if (filledGrade <= 0) {
        //     return; 
        // }
        //
        // if (hasPlayedSound) {
        //     return; 
        // }
        //
        // if (filledGrade >= 0.25) {
        //     return; 
        // }
        //
        // if (stopTimer) {
        //     hasPlayedSound = false; 
        //     return; 
        // }
        //
        // PlaySound();
        // hasPlayedSound = true; 



        // if (filledGrade <= previousFilledGrade && filledGrade > 0 && isServer && filledGrade < 0.25) {
        //     PlaySound();
        //     hasPlayedSound = true; 
        // }
        // else {
        //     hasPlayedSound = false; 
        // }

    }


    [Server]
    private void PlaySound() {
        var audioManager = WaterballAudioManager.Instance; 
        audioManager.PlaySoundEffect(audioManager.pop, 1);
    }


    [ClientRpc]
    public void StartTimerClientRpc() {
        StartCoroutine(StartTimerCoroutine()); 
    }
    
    
    private IEnumerator StartTimerCoroutine() {
        stopTimer = false; 
        filledGrade = 0;
        tStart = Time.time;
        tEnd = tStart + duration;
        canvas.SetActive(true);
        // StartCoroutine(ShowCountdown(duration));

        while (!stopTimer && Time.time < tEnd + 0.5f) {
            yield return null;
        }

        canvas.SetActive(false);
        // yield return new WaitForSeconds(duration);
        
        
    }
    
    [ClientRpc]
    public void StopTimerClientRpc() {

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




        

        