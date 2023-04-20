using System;
using Mirror;
using UnityEngine;
using UnityEngine.Networking.Types;

public class WaterballAudioManager : NetworkBehaviour {

    public static WaterballAudioManager Instance;

    public AudioClip positiveBlip;
    public AudioClip negativeBlip;
    public AudioClip yay;
    public AudioClip three; 
    public AudioClip two; 
    public AudioClip one; 
    public AudioClip jump; 
    public AudioClip thud;
    public AudioClip thud2;
    public AudioClip pop;
    public AudioClip splash; 

    public AudioSource audioSource;

    public bool isMuted = true;  
    
    

    private void Awake() {
        if (Instance == null) {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }

        else {
            if (Instance != this) {
                Destroy(gameObject);
            }
        }
        
    }

    [Server] 
    public void PlaySoundEffect(AudioClip clip, float volume)  {
        if (isMuted) {
            return; 
        }
        // Debug.Log("vi är inne i playsoundeffect");
        Debug.Log(clip);
        audioSource.PlayOneShot(clip, volume);
        
    }


    // public void Mute() {
        
    // }
    
    
}