using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Gameplay values")]
    [SerializeField] Vector3 m_offsetFromPlayer = Vector3.zero;
    [SerializeField] Transform m_spotlight = null;

    private GameObject m_player;

    void Start()
    {
        m_player = GameObject.FindWithTag("Player");
    }

    // Called after the position of the player has been processed during the Update
    void LateUpdate()
    {
        // Lock the camera spotlight to the player position
        if (m_player!= null)
        {
            m_spotlight.LookAt(m_player.transform);

            // Given an offset, lock the camera x and z position on the player
            Vector3 lockedCameraPosition = new Vector3(m_player.transform.localPosition.x + m_offsetFromPlayer.x, transform.localPosition.y, m_player.transform.localPosition.z + m_offsetFromPlayer.z);
            transform.localPosition = lockedCameraPosition;
        }
    }
}
