using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;

public class ErrorBehaviour : MonoBehaviour {
    float startTime;
    public float minWaitTime = 2f;

    public void OnEnable(){
		// Restart everything in a very destructive way
		startTime = Time.realtimeSinceStartup;		
		CITENetworkManager manager = Object.FindObjectOfType<CITENetworkManager>();
		if (manager==null){
			Debug.Log("Could not find the network manager?!?");
		} else {
			Debug.Log("Destroying NetworkManager");
			Destroy(manager.gameObject);	
		}
    }

    public void Update(){
		if (Time.realtimeSinceStartup-startTime > minWaitTime){
			SceneManager.LoadScene(0, LoadSceneMode.Single);
		}
    }
}
