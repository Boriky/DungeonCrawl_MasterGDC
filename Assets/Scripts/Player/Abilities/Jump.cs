using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : Ability
{
    public float m_force = 5.0f;
    public float m_force2 = 20.0f;
    public float m_rayLenght = 0.6f;

    private Rigidbody m_playerRb = null;
    private static Vector3 s_rayDirection = Vector3.down;
    private bool m_isSmashJumpReady = false;

    void Awake()
    {
        m_playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Stores everything the raycast is hitting
        RaycastHit hit;
        // Check if the raycast intersects with anything in its lenght
        if (Physics.Raycast(transform.position, s_rayDirection, out hit, m_rayLenght))
        {
            m_isSmashJumpReady = false;
            // Check if the raycast is hitting the floor and execute che jump command
            if (hit.collider.tag == "Floor" && Input.GetKeyDown(KeyCode.Space))
            {
                m_playerRb.AddForce(Vector3.up * m_force, ForceMode.Impulse);
                m_isSmashJumpReady = true;
            }
        }
        else
        {
            // Double jump: after the first jump the player can jump again and perform a smash down towards the enemy
            if (m_isSmashJumpReady && Input.GetKeyDown(KeyCode.Space))
            {
                m_playerRb.velocity = Vector3.zero;
                m_playerRb.AddForce(Vector3.down * m_force2, ForceMode.Impulse);
            }
        }
    }
}
