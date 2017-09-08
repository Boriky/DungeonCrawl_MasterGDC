using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    private Camera m_mainCamera = null;

	// Use this for initialization
	void Awake ()
    {
        m_mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(m_mainCamera.transform);
	}
}
