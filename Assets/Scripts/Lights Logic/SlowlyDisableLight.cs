using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Light associated to this component slowly fades off
public class SlowlyDisableLight : MonoBehaviour
{
    [Header("Object referece")]
    [SerializeField] GameObject LightCone = null;

    [Header("Gameplay values")]
    [SerializeField] float m_fadeVelocity = 0.008f;

    [Header("Utility values")]
    public bool m_deactivating = false;

    private Light m_pointLight = null;

    private void Start()
    {
        m_pointLight = GetComponent<Light>();
    }
	
	void Update ()
    {
        if (m_deactivating)
        {
            LightCone.SetActive(false);
            m_pointLight.intensity -= m_fadeVelocity; 
        }
    }
}
