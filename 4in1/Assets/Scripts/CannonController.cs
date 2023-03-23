using System;
using UnityEngine;


public class CannonController : MonoBehaviour {
    public GameObject angledBarrel;
    public GameObject tower;


    private bool isRotating = false;


    // private Vector3 lastMousePosition;
    private Quaternion initialTowerRotation;
    private Quaternion initialBarrelRotation;

    private Vector3 initialMousePosition;

    private void Start() {
        initialTowerRotation = tower.transform.localRotation;
        initialBarrelRotation = angledBarrel.transform.localRotation;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            isRotating = true;
            initialMousePosition = Input.mousePosition;
            initialTowerRotation = tower.transform.localRotation;
            initialBarrelRotation = angledBarrel.transform.localRotation;
        }

        else if (Input.GetMouseButtonUp(0)) {
            isRotating = false;
        }

        if (isRotating) {
            Vector3 currentMousePosition = Input.mousePosition;
            float deltaX = (currentMousePosition.x - initialMousePosition.x) * 0.1f;
            float deltaY = (currentMousePosition.y - initialMousePosition.y) * 0.1f;

            // Rotate the tower horizontally
            Quaternion horizontalRotation = Quaternion.Euler(0f, deltaX, 0f);
            Quaternion newTowerRotation = initialTowerRotation * horizontalRotation;
            tower.transform.localRotation = newTowerRotation;

            // Rotate the barrel vertically
            Quaternion verticallRotation = Quaternion.Euler(deltaY, 0f, 0f);
            Quaternion newBarrelRotation = initialBarrelRotation * verticallRotation;
            angledBarrel.transform.localRotation = newBarrelRotation;
            
            // Rotate the barrel horizontally relative to the tower
            Quaternion barrelHorizontalRotation = Quaternion.Euler(0f, deltaX, 0f);
            Quaternion newBarrelLocalRotation = angledBarrel.transform.localRotation * barrelHorizontalRotation;
            angledBarrel.transform.localRotation = newBarrelLocalRotation;
            
        }
    }
}