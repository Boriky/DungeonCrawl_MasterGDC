using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowlyDisableLight : MonoBehaviour {

    [Header("Gameplay values")]
    [SerializeField] float m_fadeVelocity = 0.008f;

    [Header("Utility values")]
    public bool m_deactivating = false;

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
