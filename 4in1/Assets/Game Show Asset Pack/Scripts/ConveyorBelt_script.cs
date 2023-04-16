using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class ConveyorBelt_script : MonoBehaviour
{
    public float speed;
    public Vector3 direction;
    public List<GameObject> onBelt;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // for (int i = 0; i <= onBelt.Count - 1; i++)
        // {
            // var initialVelocity = onBelt
            // onBelt[i].GetComponent<Rigidbody>().velocity = speed * direction;
        // }


        foreach (var thing in onBelt) {
            var force = direction * speed * Time.deltaTime; 
            var rb = thing.GetComponent<Rigidbody>(); 
            rb.MovePosition(rb.position + force);
                
                
                
                // .AddForce(force, ForceMode.Impulse);
            
            
            


            // var oldPosition = thing.transform.position;
            // var delta = speed * direction * Time.deltaTime; 
            // thing.transform.position = oldPosition + delta; 


            // var initialVelocity = thing.GetComponent<Rigidbody>().velocity;
            // var newVelocity = initialVelocity + speed * direction;
            // thing.GetComponent<Rigidbody>().velocity = newVelocity; 
            // }
        }
        
    }

    // When something collides with the belt
    private void OnCollisionEnter(Collision collision)
    {
        onBelt.Add(collision.gameObject);
    }

    // When something leaves the belt
    private void OnCollisionExit(Collision collision)
    {
        onBelt.Remove(collision.gameObject);
    }
}
