using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Gameplay values")]
    [SerializeField]
    bool m_keyboardControls = true;
    [SerializeField]
    float m_movementSpeed = 5.0f;

    private Camera m_mainCamera = null;
    private Rigidbody m_playerRb;

    // Use this for initialization
    void Start ()
    {
        m_mainCamera = Camera.main;
        m_playerRb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        float moveOnXAxis;
        float moveOnZAxis;

        if (m_keyboardControls)
        {
            moveOnXAxis = Input.GetAxis("Horizontal");
            moveOnZAxis = Input.GetAxis("Vertical");
        }
        else
        {
            moveOnXAxis = (Input.acceleration.x /*- m_zStart*/);
            moveOnZAxis = (Input.acceleration.z /*- m_xStart*/);
        }
        Vector3 direction = new Vector3(moveOnXAxis, 0.0f, moveOnZAxis);
        Vector3 rotated = m_mainCamera.transform.rotation * direction;
        m_playerRb.AddForce(rotated * m_movementSpeed, ForceMode.Force);
    }
}
