using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLightCrossed : MonoBehaviour {

    Transform m_divineLightPosition = null;
    GameObject hit = null;
    public bool hasEntered = false;

    private void Update()
    {
        if (hasEntered)
        {
            Vector3 gravityOrigin = m_divineLightPosition.position;

            Vector3 toGravityOriginFromPlayer = gravityOrigin - gameObject.transform.position;
            //toGravityOriginFromPlayer.Normalize();

            // Multiply vector so that the magnitude is equal to the force we wish to apply
            /*float accelerationDueToGravity = 9.8f;
            toGravityOriginFromPlayer *= accelerationDueToGravity * gameObject.GetComponent<Rigidbody>().mass * Time.deltaTime;*/

            //Apply our acceleration.
            gameObject.GetComponent<Rigidbody>().AddForce(toGravityOriginFromPlayer * 4.0f, ForceMode.Acceleration);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        hit = other.gameObject;

        if ("LevelTrigger" == hit.tag)
        {
            hasEntered = true;
            m_divineLightPosition = hit.transform.parent.transform;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "LevelTrigger2")
        {
            GetComponent<Rigidbody>().isKinematic = true;       }
    }
}
