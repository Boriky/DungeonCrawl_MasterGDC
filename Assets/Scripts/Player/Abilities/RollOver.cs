using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollOver : Ability
{
    [Header("Gameplay values")]
    [SerializeField] float m_force = 5.0f;
    [SerializeField] bool m_keyboardControls = true;

    private Rigidbody m_playerRb = null;

	// Use this for initialization
	void Awake ()
    {
        m_playerRb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
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

        Vector3 movementDirection = new Vector3(xAxis, 0, zAxis);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_playerRb.AddForce(movementDirection * m_force, ForceMode.VelocityChange);
        }
	}
}
