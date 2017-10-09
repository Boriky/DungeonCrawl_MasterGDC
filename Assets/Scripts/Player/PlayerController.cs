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
    [SerializeField]
    float m_maxVelocity = 10.0f;

    private Camera m_mainCamera = null;
    private Rigidbody m_playerRb;

    // Use this for initialization
    void Start ()
    {
        m_mainCamera = Camera.main;
        m_playerRb = GetComponent<Rigidbody>();
        m_playerRb.maxAngularVelocity = 100;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
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
            moveOnZAxis = (Input.acceleration.y /*- m_xStart*/);
        }
        Vector3 direction = new Vector3(moveOnZAxis, 0.0f, -moveOnXAxis);
        Quaternion cameraOrientation = new Quaternion(0.0f, m_mainCamera.transform.rotation.y, 0.0f, m_mainCamera.transform.rotation.w);
        Vector3 rotated = cameraOrientation * direction.normalized;
        if (m_playerRb.velocity.magnitude < m_maxVelocity)
        {
            m_playerRb.AddTorque(rotated.normalized * m_movementSpeed, ForceMode.Force);
        }
    }
}
