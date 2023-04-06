using System;
using UnityEngine;

namespace DefaultNamespace {
    public class WaterballBanner : MonoBehaviour {


        public float rotationSpeed = 30f;

        private void Update() {
            transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));
        }
    }
}