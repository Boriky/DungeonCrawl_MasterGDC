using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Make a GameObject (like a billboard or an UI element) always face the main camera
public class LookAtCamera : MonoBehaviour
{
    private Camera m_mainCamera = null;

	void Awake ()
    {
        m_mainCamera = Camera.main;
	}
	
	void Update ()
    {
        transform.LookAt(m_mainCamera.transform);
	}
}
