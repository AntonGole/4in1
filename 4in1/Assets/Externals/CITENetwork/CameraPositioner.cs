using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * Automatically position the camera based on device screen
 */
[ExecuteInEditMode]
public class CameraPositioner : MonoBehaviour {
    public enum PLACEMENT {
        SQUARE,
        ROW
    };

    public static readonly float[,] PLACEMENT_SQUARE = {{-0.5f, 0.5f}, {0.5f, 0.5f}, {-0.5f, -0.5f}, {0.5f, -0.5f}};
    public static readonly float[,] PLACEMENT_ROW = {{-1.5f, 0}, {-0.5f, 0}, {0.5f, 0}, {1.5f, 0}};

    [Tooltip(
        "Number < 1 means overlap (the same object can be shown on more than 1 camera), > 1 means borders (space that no camera shows)")]
    [Range(0.0f, 2.0f)]
    public float spacing = 1f;

    [Tooltip(
        "0: No sharing, each camera looks directly at view, 1: full sharing, all views act as segments of 1 camera")]
    [Range(0.0f, 1.0f)]
    public float frustumSharing = 0;

    [Tooltip("World-space units that the camera will calculate view size for")]
    public float distance = 10;

    [Tooltip("Algorithm used for placing cameras")]
    public PLACEMENT placement = PLACEMENT.SQUARE;


    public GameObject floor;



    private void Update() {
        float[,] placementArray;
        switch (placement) {
            case PLACEMENT.SQUARE:
                placementArray = PLACEMENT_SQUARE;
                break;
            case PLACEMENT.ROW:
                placementArray = PLACEMENT_ROW;
                break;
            default:
                throw new Exception("No such placement algorithm supported");
        }

        // int placementIndex = 0;


        var cameras = GetComponentsInChildren<Camera>(true);

        for (int i = 0; i < cameras.Length; i++) {
            setCameraPosition(cameras[i], i, floor);

        }

    }


    private void setCameraPosition(Camera camera, int index, GameObject floor) {
        float screenX = 197.12f;
        float screenZ = 147.84f;
        float bezelX = 21.44f;
        float bezelZ = 10.83f;
        float totalX = screenX* 2 + bezelX* 2;
        float totalZ = screenZ* 2 + bezelZ* 2;


        MeshRenderer renderer = floor.GetComponent<MeshRenderer>();

        Bounds bounds = renderer.bounds;
        var delta_x = bounds.extents.x;
        var delta_y = bounds.extents.y;
        var delta_z = bounds.extents.z;

        float relativeScreenX = (delta_x * 2 / totalX * screenX);
        float relativeScreenZ = (delta_z * 2 / totalZ * screenZ);


        Vector3 corner, cameraPosition;


        switch (index) {
            case 0:
                corner = bounds.center + new Vector3(-delta_x, delta_y * 2, delta_z);
                cameraPosition = corner + new Vector3(relativeScreenX / 2, 0, -relativeScreenZ / 2);
                break;
            case 1:
                corner = bounds.center + new Vector3(delta_x, delta_y * 2, delta_z);
                cameraPosition = corner + new Vector3(-relativeScreenX / 2, 0, -relativeScreenZ / 2);
                break;
            case 2:
                corner = bounds.center + new Vector3(-delta_x, delta_y * 2, -delta_z);
                cameraPosition = corner + new Vector3(relativeScreenX / 2, 0, relativeScreenZ / 2);
                break;
            case 3:
                corner = bounds.center + new Vector3(delta_x, delta_y * 2, -delta_z);
                cameraPosition = corner + new Vector3(-relativeScreenX / 2, 0, relativeScreenZ / 2);
                break;
            default:
                corner = bounds.center + new Vector3(0, delta_y, 0);
                cameraPosition = corner + new Vector3(0, 0,0 );
                break;
        }
        

        var cameraHeight = relativeScreenZ / 2;
        var cameraTransform = camera.transform; 
        
        cameraTransform.localRotation = Quaternion.identity;
        cameraTransform.position = cameraPosition + new Vector3(0, 10, 0);
        camera.orthographic = false;
        camera.orthographicSize = cameraHeight; 

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




// foreach (Camera child in GetComponentsInChildren<Camera>(true))
// {
//     // STUB: Also handle orthogonal projection cameras
//     float heightAtDistance = 2.0f * distance * Mathf.Tan(child.fieldOfView * 0.5f * Mathf.Deg2Rad);
//     float widthAtDistance = heightAtDistance * child.aspect;
//
//     //Debug.Log("placing "+placementIndex+" at "+placementArray[placementIndex,0]+","+placementArray[placementIndex,1]);
//     child.transform.localRotation = Quaternion.identity;
//     child.transform.localPosition = new Vector3(placementArray[placementIndex, 0] * widthAtDistance * spacing * (1f - frustumSharing), placementArray[placementIndex, 1] * heightAtDistance * spacing * (1f - frustumSharing), 0);
//     child.lensShift = new Vector2(placementArray[placementIndex, 0] * frustumSharing * spacing, placementArray[placementIndex, 1] * frustumSharing * spacing);
//     placementIndex++;
// }





//         Vector3 corner0 = bounds.center + new Vector3(-delta_x, delta_y, delta_z);
//         Vector3 corner1 = bounds.center + new Vector3(delta_x, delta_y, delta_z);
//         Vector3 corner2 = bounds.center + new Vector3(-delta_x, delta_y, -delta_z);
//         Vector3 corner3 = bounds.center + new Vector3(delta_x, delta_y, -delta_z);
//         Vector3 default_position = bounds.center + new Vector3(0, delta_y, 0);



//
//
//
//     private float totalX, totalY;
//     
//     
//
//
//
//
//
//
// MeshRenderer renderer = floor.GetComponent<MeshRenderer>(); 
//         // Vector3[] corners = new Vector3[8];
//         Bounds bounds = renderer.bounds; 
//         // renderer.bounds.extents.x, 
//
//         var delta_x = bounds.extents.x;
//         var delta_y = bounds.extents.y;
//         var delta_z = bounds.extents.z;
//
//         Vector3 corner0 = bounds.center + new Vector3(-delta_x, delta_y, delta_z);
//         Vector3 corner1 = bounds.center + new Vector3(delta_x, delta_y, delta_z);
//         Vector3 corner2 = bounds.center + new Vector3(-delta_x, delta_y, -delta_z);
//         Vector3 corner3 = bounds.center + new Vector3(delta_x, delta_y, -delta_z);
//         Vector3 default_position = bounds.center + new Vector3(0, delta_y, 0);
//         
//         switch (index) {
//             case 0:
//                 var corner = bounds.center + new Vector3(-delta_x, delta_y, delta_z);
//                 
//                 
//                 
//                 
//                 return corner0;
//                 break; 
//             case 1:
//                 return corner1;
//                 break; 
//             case 2:
//                 return corner2;
//                 break; 
//             case 3:
//                 return corner3;
//                 break; 
//             default:
//                 return default_position; 
//     
    

    