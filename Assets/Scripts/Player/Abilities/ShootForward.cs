using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootForward : Ability
{
    public bool m_keyboardControls = true;
    public float m_force = 50.0f;
    public GameObject m_projectile = null;
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float zAxis;
        float xAxis;

        if (m_keyboardControls)
        {
            zAxis = Input.GetAxis("Vertical");
            xAxis = Input.GetAxis("Horizontal");
        }
        else
        {
            zAxis = (Input.acceleration.z /*- m_zStart*/);
            xAxis = (Input.acceleration.y /*- m_xStart*/);
        }

        if ((zAxis != 0 || xAxis != 0) && Input.GetKeyDown(KeyCode.F))
        {
            FireProjectile(zAxis, xAxis);
        }
    }

    void FireProjectile(float zAxis, float xAxis)
    {
        Vector3 movementDirection = new Vector3(xAxis, 0, zAxis);

        float zOffset = 0.0f;
        float xOffset = 0.0f;

        if (zAxis > 0)
        {
            zOffset = 2;
        }
        else if (zAxis < 0)
        {
            zOffset = -2;
        }

        if (xAxis > 0)
        {
            xOffset = 2;
        }
        else if (xAxis < 0) 
        {
            xOffset = -2;
        }

        Vector3 spawnLocation = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset);

        GameObject projectile = Instantiate(m_projectile, spawnLocation, Quaternion.identity);
        projectile.transform.parent = GameObject.Find("Room(Clone)").transform;
        Rigidbody projRb = projectile.GetComponent<Rigidbody>();
        projRb.velocity = movementDirection * m_force;
    }
}
