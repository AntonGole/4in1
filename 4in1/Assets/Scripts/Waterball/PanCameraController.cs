using System;
using Mirror;
using UnityEngine;
using UnityEngine.Networking.Types;


public class PanCameraController : MonoBehaviour {

    public string outputFolder = "Captures";  // Output folder for captured frames
    public int frameRate = 60;  // Frame rate for capturing frames
    private bool isRecording = true;  // Flag to indicate if recording is in progress



    public GameObject centerPoint;
    public float radius = 10f;
    public float speed = 0.02f;
    public float height = 5f; 
    private Transform centerPointTransform; 
    

    private void Start() {

        centerPointTransform = centerPoint.transform; 

    }

    private void Update() {

        Vector3 circlePosition =
            new Vector3(Mathf.Sin(Time.time * speed) * radius, 0f, Mathf.Cos(Time.time * speed) * radius);
        Vector3 heightDiff = new Vector3(0, height, 0); 
        
        Vector3 desiredPosition = centerPointTransform.position + heightDiff + circlePosition;
        
        
        transform.position = desiredPosition;
        transform.LookAt(centerPointTransform);
        
        
        
        
        
    }
}