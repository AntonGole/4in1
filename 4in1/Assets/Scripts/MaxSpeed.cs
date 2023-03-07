using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSpeed : MonoBehaviour
{
    public float maxSpeed = 5f; //Replace with your max speed
    public float minSpeed = 0f; //Replace with your max speed

    private new Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rigidbody.velocity.magnitude > maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
        }
        else if (rigidbody.velocity.magnitude < minSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * minSpeed;
        }
    }
}
