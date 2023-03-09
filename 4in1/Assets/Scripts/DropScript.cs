using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropScript : MonoBehaviour
{
    [SerializeField] Slider slider;


    Rigidbody rb;
    float force;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        force = slider.value;
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }

    
}
