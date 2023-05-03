using UnityEngine;
using Mirror;

public class WaterballTitleBanner : MonoBehaviour {
    public float circularBannerRotationSpeed = 30f;

    private void Update() {
        // if (!isServer) {
            // return;
        // }
        
        RotateBanner();
    }

    // [Server]
    private void RotateBanner() {
        transform.Rotate(new Vector3(0, 0, -circularBannerRotationSpeed * Time.deltaTime));
    }
}