using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    [SerializeField]
    float m_scaleFactor = 0.0f;
    [SerializeField]
    float m_runningTime = 0.0f;
    [SerializeField]
    bool m_isRotating = false;

    // Update is called once per frame
    void Update ()
    {
        Vector3 newLocation = transform.position;
        float deltaHeight = Mathf.Sin(m_runningTime + Time.deltaTime) - Mathf.Sin(m_runningTime);
        newLocation.y += deltaHeight * m_scaleFactor;
        m_runningTime += Time.deltaTime;
        transform.position = newLocation;

        if (m_isRotating)
        {
            transform.Rotate(0, 6.0f * 10.0f * Time.deltaTime, 0);
        }
	}
}
