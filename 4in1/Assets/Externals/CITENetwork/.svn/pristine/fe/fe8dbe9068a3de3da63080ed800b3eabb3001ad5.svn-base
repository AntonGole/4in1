using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * Automatically position the camera based on device screen
 */
[ExecuteInEditMode]
public class CameraPositioner : MonoBehaviour
{
    public enum PLACEMENT { SQUARE, ROW };
    public static readonly float[,] PLACEMENT_SQUARE = { { -0.5f, 0.5f }, { 0.5f, 0.5f }, { -0.5f, -0.5f }, { 0.5f, -0.5f } };
    public static readonly float[,] PLACEMENT_ROW = { { -1.5f, 0 }, { -0.5f, 0 }, { 0.5f, 0 }, { 1.5f, 0 } };

    [Tooltip("Number < 1 means overlap (the same object can be shown on more than 1 camera), > 1 means borders (space that no camera shows)")]
    [Range(0.0f, 2.0f)]
    public float spacing = 1f;

    [Tooltip("0: No sharing, each camera looks directly at view, 1: full sharing, all views act as segments of 1 camera")]
    [Range(0.0f, 1.0f)]
    public float frustumSharing = 0;

    [Tooltip("World-space units that the camera will calculate view size for")]
    public float distance = 10;

    [Tooltip("Algorithm used for placing cameras")]
    public PLACEMENT placement = PLACEMENT.SQUARE;

    private void Update()
    {
        float[,] placementArray;
        switch (placement)
        {
            case PLACEMENT.SQUARE:
                placementArray = PLACEMENT_SQUARE;
                break;
            case PLACEMENT.ROW:
                placementArray = PLACEMENT_ROW;
                break;
            default:
                throw new Exception("No such placement algorithm supported");
        }

        int placementIndex = 0;
        foreach (Camera child in GetComponentsInChildren<Camera>(true))
        {
            // STUB: Also handle orthogonal projection cameras
            float heightAtDistance = 2.0f * distance * Mathf.Tan(child.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float widthAtDistance = heightAtDistance * child.aspect;

            //Debug.Log("placing "+placementIndex+" at "+placementArray[placementIndex,0]+","+placementArray[placementIndex,1]);
            child.transform.localRotation = Quaternion.identity;
            child.transform.localPosition = new Vector3(placementArray[placementIndex, 0] * widthAtDistance * spacing * (1f - frustumSharing), placementArray[placementIndex, 1] * heightAtDistance * spacing * (1f - frustumSharing), 0);
            child.lensShift = new Vector2(placementArray[placementIndex, 0] * frustumSharing * spacing, placementArray[placementIndex, 1] * frustumSharing * spacing);
            placementIndex++;
        }
    }

    public void setView(int viewID)
    {
        int placementIndex = 0;
        foreach (Camera child in GetComponentsInChildren<Camera>(true))
        {
            if(placementIndex == viewID)
            {
                child.enabled = true;
            } else
            {
                child.enabled = false;
            }


            placementIndex++;
        }
    }
}
