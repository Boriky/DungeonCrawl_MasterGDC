using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float cameraSpeed = 20.0f;
    [SerializeField] Vector3 m_offset = Vector3.zero;
    [SerializeField] Transform m_spotlight = null;

    private GameObject m_player;

    void Start()
    {
        m_player = GameObject.FindWithTag("Player");
    }

    // Called after the position of the player has been processed during the Update
    void LateUpdate()
    {
        m_spotlight.LookAt(m_player.transform);
        transform.localPosition = new Vector3(m_player.transform.localPosition.x + m_offset.x, transform.localPosition.y, m_player.transform.localPosition.z + m_offset.z);

        /*
        transform.LookAt(m_player.transform);

        if (Input.GetKey(KeyCode.L))
        {
            transform.RotateAround(m_player.transform.position, -Vector3.up, cameraSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.J))
        {
            transform.RotateAround(m_player.transform.position, Vector3.up, cameraSpeed * Time.deltaTime);
        }
        */
    }
}
