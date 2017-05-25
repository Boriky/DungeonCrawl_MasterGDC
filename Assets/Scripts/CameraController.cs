using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject m_player;

    private Vector3 m_offset;

    // Use this for initialization
    void Start() {
        m_offset = transform.position - m_player.transform.position;
    }

    // Called after the position of the player has been processed during the Update
    void LateUpdate() {
        transform.position = m_player.transform.position + m_offset;
    }
}
