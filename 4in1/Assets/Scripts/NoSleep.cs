using UnityEngine;

public class NoSleep : MonoBehaviour
{
    void Start()
    {
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}