﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Jump : MonoBehaviour
{
    public float m_force = 5f;
    public float m_rayLenght = 0.6f;

    private Rigidbody m_playerRb = null;
    private static Vector3 s_rayDirection = Vector3.down;

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
            // Check if the raycast is hitting the floor and execute che jump command
            if(hit.collider.tag == "Floor" && Input.GetKeyDown(KeyCode.Space))
            {
                m_playerRb.AddForce(Vector3.up * m_force, ForceMode.Impulse);
            }
        }
    }
}
