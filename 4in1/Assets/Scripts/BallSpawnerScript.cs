using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject Ball;
    Vector3 StartPos;
    Quaternion StartRot;

    void Start()
    {
        StartPos = gameObject.transform.position-new Vector3(0,1,0);
        StartRot = gameObject.transform.rotation;
        

    }

    void Update()
    {

        if (Ball.transform.position.y < -3)
        {
            Destroy(Ball, 2f);
            //var ball =
            Ball = Instantiate(Ball, StartPos, StartRot);
        }
    }
}
