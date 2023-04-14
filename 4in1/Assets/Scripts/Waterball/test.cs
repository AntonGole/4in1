    using System;
    using UnityEngine;

    public class test : MonoBehaviour {



        public int x_direction;
        public int z_direciton; 
        
        private void Start() {

            




        }

        
        

        private void Update() {
            
            
            
            
            Quaternion looking = Quaternion.LookRotation(new Vector3(x_direction, 0, z_direciton));
            
            // transform.
            
        }
    }
