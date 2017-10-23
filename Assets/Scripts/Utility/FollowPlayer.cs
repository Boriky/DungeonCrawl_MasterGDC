using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Vector3 m_offset = Vector3.zero;

    Transform m_playerTransform = null;

    private void Start()
    {
        m_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update ()
    {
        transform.position = m_playerTransform.position + m_offset;

        transform.Rotate(0, 6.0f * 10.0f * Time.deltaTime, 0);
    }
}
