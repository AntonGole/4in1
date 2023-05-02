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