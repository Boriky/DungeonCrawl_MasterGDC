using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float cameraSpeed = 20.0f;

    private GameObject m_player;
    //private Vector3 m_offset;

    void Start()
    {
        m_player = GameObject.FindWithTag("Player");
        //m_offset = transform.position - m_player.transform.position;
    }

    // Called after the position of the player has been processed during the Update
    void LateUpdate()
    {
        if (m_player == null)
            return;

        transform.LookAt(m_player.transform);

        if (Input.GetKey(KeyCode.L))
        {
            transform.RotateAround(m_player.transform.position, -Vector3.up, cameraSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.J))
        {
            transform.RotateAround(m_player.transform.position, Vector3.up, cameraSpeed * Time.deltaTime);
        }
    }
}
