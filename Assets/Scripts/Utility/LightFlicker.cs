using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Header("Gameplay values")]
    [SerializeField]
    float m_minLightIntensity = 0.1f;
    [SerializeField]
    float m_maxLightIntensity = 5.0f;
    [SerializeField]
    float m_lightRiseConstant = 0.01f;

    private Light m_light;
    private bool rise;

    // Use this for initialization
    void Start ()
    {
        m_light = GetComponent<Light>();
        m_light.intensity = m_maxLightIntensity + 0.1f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_light.intensity > m_maxLightIntensity)
        {
            rise = false;
        }
        else if (m_light.intensity < m_minLightIntensity)
        {
            rise = true;
        }

        if (rise)
            m_light.intensity += m_lightRiseConstant;
        else
            m_light.intensity -= m_lightRiseConstant;
    }
}
