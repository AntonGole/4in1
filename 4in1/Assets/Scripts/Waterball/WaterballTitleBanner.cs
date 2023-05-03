using UnityEngine;
using Mirror;

public class WaterballTitleBanner : NetworkBehaviour {
    public float circularBannerRotationSpeed = 30f;

    private void Update() {
        if (!isServer) {
            return;
        }


        // Debug.Log("jag är server inne i rotatebanner");
        
        RotateBanner();
    }

    // [Server]
    private void RotateBanner() {
        transform.Rotate(new Vector3(0, 0, -circularBannerRotationSpeed * Time.deltaTime));
    }
}