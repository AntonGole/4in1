using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * Automatically position the camera based on device screen
 */
[ExecuteInEditMode]
public class WaterballCameraPositioner : MonoBehaviour {


    public GameObject floor; 
    public static readonly float[,] PLACEMENT_RECTANGLE = { { -1f, 1f }, { 1f, 1f }, { -1f, -1f }, { 1f, -1f } };
    public float spacing = 1f;
    public float frustumSharing = 0;
    public float distance = 10;
    public float shortSideWidth = 147.84f;
    public float shortSideHeight = 317.34f;
    public float longSideWidth = 197.12f;
    public float longSideHeight = 437.12f;
    public float gapWidth = 10f;

    private void Update()
    {
        float[,] placementArray = PLACEMENT_RECTANGLE;

        int placementIndex = 0;
        foreach (Camera child in GetComponentsInChildren<Camera>(true))
        {
            float x = placementArray[placementIndex, 0];
            float y = placementArray[placementIndex, 1];

            float shortSideLength = Mathf.Lerp(shortSideWidth, shortSideHeight, Mathf.Abs(y));
            float longSideLength = Mathf.Lerp(longSideWidth, longSideHeight, Mathf.Abs(x));
            float widthAtDistance = longSideLength * spacing;
            float heightAtDistance = shortSideLength * spacing;

            float xPos = x * (longSideLength * spacing + gapWidth);
            float yPos = y * (shortSideLength * spacing + gapWidth);

            child.transform.localRotation = Quaternion.identity;
            child.transform.localPosition = new Vector3(xPos, yPos, 0);
            child.lensShift = new Vector2(xPos * frustumSharing, yPos * frustumSharing);

            placementIndex++;
        }
    }

    public void setView(int viewID)
    {
        int placementIndex = 0;
        foreach (Camera child in GetComponentsInChildren<Camera>(true))
        {
            if (placementIndex == viewID)
            {
                child.enabled = true;
            }
            else
            {
                child.enabled = false;
            }


            placementIndex++;
        }
    }
}