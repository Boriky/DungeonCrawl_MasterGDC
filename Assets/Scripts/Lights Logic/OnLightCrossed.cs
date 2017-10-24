using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Check if the player is inside the divine light spawned once the level has been cleared by all the enemies
public class OnLightCrossed : MonoBehaviour
{
    [Header("Gameplay values")]
    [SerializeField] float m_attractiveForce = 2.0f;

    private Transform m_divineLightPosition = null;
    private GameObject hit = null;
    public bool hasEntered = false;

    private void Update()
    {
        if (hasEntered)
        {
            Vector3 gravityOrigin = m_divineLightPosition.position;

            Vector3 toGravityOriginFromPlayer = gravityOrigin - gameObject.transform.position;
            toGravityOriginFromPlayer.Normalize();

            // Apply acceleration to the player towards the "gravity center"
            gameObject.GetComponent<Rigidbody>().AddForce(toGravityOriginFromPlayer * m_attractiveForce, ForceMode.Force);
        }

    }

    /// <summary>
    /// Check if the player is inside the light's cone trigger
    /// </summary>
    private void OnTriggerEnter(Collider trigger)
    {
        hit = trigger.gameObject;

        if ("LevelTrigger" == hit.tag)
        {
            hasEntered = true;
            m_divineLightPosition = hit.transform.parent.transform;
        }
    }

    /// <summary>
    /// When the player collides with the "new level generation" collider, lock him
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "LevelTrigger2")
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
