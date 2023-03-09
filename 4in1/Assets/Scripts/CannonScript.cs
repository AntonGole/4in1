using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonScript : MonoBehaviour
{
    [SerializeField] GameObject BallSpawnPoint;
    [SerializeField] GameObject Drop;
    [SerializeField] public Slider angleSlide;

    bool water = true;
    public float angle;
    public float old;
    public Vector3 orientation;

    public void Start()
    {
        angle = angleSlide.value;
        orientation = gameObject.transform.eulerAngles;     // Default rotation of the cannon

    }
    private void FixedUpdate()
    {
        if (water == true)
        {
            var dropp = (GameObject) Instantiate(Drop, BallSpawnPoint.transform.position, BallSpawnPoint.transform.rotation);
            Destroy(dropp, 2f);                             // Drops disappear after 2 sec
        }


        if (Input.GetButtonDown("Fire1"))
        {
            //water = false; 
        }
            
         angleSlide.onValueChanged.AddListener( delegate { rotateCannon(); } );
        
    }
    public void rotateCannon()
    {
        float angle = angleSlide.value;
        Vector3 rotation = new Vector3(0, angle, 0) + orientation;   // new rotation = start + angle
        transform.rotation = Quaternion.Euler(rotation);



    }
}
