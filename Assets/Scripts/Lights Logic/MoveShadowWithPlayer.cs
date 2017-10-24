using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Move the fake projector's shadow with the player position
public class MoveShadowWithPlayer : MonoBehaviour
{
    private GameObject m_player = null;
    private float m_offsetFromPlayer = 5.0f;

    private void Start()
    {
        m_player = GameObject.FindWithTag("Player");
    }

	void Update ()
    {
        float x = m_player.transform.position.x;
        float y = m_player.transform.position.y + m_offsetFromPlayer;
        float z = m_player.transform.position.z;
        transform.position = new Vector3(x, y, z);
	}
}
