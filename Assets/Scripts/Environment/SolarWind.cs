using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityStandardAssets.Vehicles.Car
{
    public class SolarWind : MonoBehaviour
    {
        [SerializeField] private float pulse;
        private void Update()
        {
          //  this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 5 * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.CompareTag("Car"))
            {
                Vector3 force = new Vector3(0, 0, -pulse);
                other.GetComponentInParent<Rigidbody>().drag = 1;
                //other.GetComponentInParent<CarUserControl>().isCarJammed = true;
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.CompareTag("Car"))
            {
                other.GetComponentInParent<Rigidbody>().drag = 0.1f;
                //other.GetComponentInParent<CarUserControl>().isCarJammed = false;
            }
        }
    }

}