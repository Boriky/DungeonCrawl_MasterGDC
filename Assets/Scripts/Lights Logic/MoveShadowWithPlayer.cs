using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShadowWithPlayer : MonoBehaviour
{
    private GameObject m_playerInstance = null;
    private float m_offsetFromPlayer = 5.0f;

    private void Start()
    {
        m_playerInstance = GameObject.FindWithTag("Player");
    }

	// Update is called once per frame
	void Update ()
    {
        float x = m_playerInstance.transform.position.x;
        float y = m_playerInstance.transform.position.y + m_offsetFromPlayer;
        float z = m_playerInstance.transform.position.z;
        transform.position = new Vector3(x, y, z);
	}
}
