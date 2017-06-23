using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollOver : Ability {

    public float m_force = 5.0f;

    private Rigidbody m_playerRb = null;

	// Use this for initialization
	void Awake ()
    {
        m_playerRb = GetComponent<Rigidbody>();
    }

    void Start()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_playerRb.AddForce(m_playerRb.velocity * m_force, ForceMode.Impulse);
        }
	}
}
