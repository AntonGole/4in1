using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sonjas_Rotation_script : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float down_angle;
    [SerializeField] private float up_angle;
    [SerializeField] private float orientation;
    float my_rot;


    // Update is called once per frame
    void Start()
    {
        my_rot = _rotation.y;

    }

    void Update()
    {

        transform.Rotate(new Vector3(0, my_rot, 0) * Time.deltaTime);
        if (gameObject.transform.rotation.eulerAngles.y > down_angle)
        {

            gameObject.transform.rotation.eulerAngles.Set(0, down_angle, 0);
            my_rot = -_rotation.y;
            if (orientation > 0)
            {
                my_rot *= 3;
            }
        }
        if (gameObject.transform.rotation.eulerAngles.y < up_angle)
        {
            gameObject.transform.rotation.eulerAngles.Set(0, up_angle, 0);
            my_rot = _rotation.y;
            if (orientation < 0)
            {
                my_rot *= 3;
            }

        }

     {
        }

    }
}
