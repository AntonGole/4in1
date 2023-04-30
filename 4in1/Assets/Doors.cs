using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public Rigidbody rb;
    public bool open;
    public float direction;
    private float x;
    private float z;
    private float y;
    private Quaternion rotation;


    // Start is called before the first frame update
    void Start()
    {
        //rb = gameObject.GetComponent<Rigidbody>();
        open = false;
        x = gameObject.transform.position.x;
        y = 0.5F ;
        z = gameObject.transform.position.z;
        rotation = gameObject.transform.rotation;
    }

    // Update is called once per frame
    IEnumerator OpenClose()
    {
        if (open==false)
        {
            //rb.AddForce(new Vector3(10*direction, 0, 0));
            
            gameObject.transform.SetPositionAndRotation(new Vector3 (x,y,z+direction), rotation);
            yield return new WaitForSeconds(5);
        }

        if (open)
        {
            gameObject.transform.SetPositionAndRotation(new Vector3(x, y, z), rotation);
            yield return new WaitForSeconds(5);
        }

    }
}
