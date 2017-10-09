using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowlyDisableLight : MonoBehaviour {

    public bool m_deactivating = false;
    public float m_fadeVelocity = 0.008f;

    private Light m_pointLight = null;

    private void Start()
    {
        m_pointLight = GetComponent<Light>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_deactivating)
        {
            m_pointLight.intensity -= m_fadeVelocity; 
        }
    }
}
