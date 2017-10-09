using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollOver : Ability
{
    [Header("Gameplay values")]
    [SerializeField] float m_force = 5.0f;
    [SerializeField] float m_rollOverDelay = 0.5f;
    [SerializeField] bool m_keyboardControls = true;
    [SerializeField] float m_cooldown = 1.0f;
    [SerializeField] float m_lightIntensityPerFrame = 0.1f;
    [SerializeField] float m_maxLightIntensity = 100.0f;
    [SerializeField] float m_defaultLightIntensity = 1.0f;

    [Header("Object References")]
    [SerializeField] Light m_playerChargeLight = null;

    private Rigidbody m_playerRb = null;
    private GameManager m_gameManager = null;
    private bool m_roolOverActivated = false;

    float zAxis;
    float xAxis;

    // Use this for initialization
    void Awake ()
    {
        m_playerRb = GetComponent<Rigidbody>();
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        m_playerChargeLight = GameObject.Find("PlayerChargeLight").GetComponent<Light>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_keyboardControls)
        {
            zAxis = Input.GetAxis("Vertical");
            xAxis = Input.GetAxis("Horizontal");
        }
        else
        {
            zAxis = (Input.acceleration.y /*- m_zStart*/);
            xAxis = (Input.acceleration.x /*- m_xStart*/);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            PerformRollOver();
        }

        if (m_roolOverActivated)
        {
            m_playerRb.velocity = Vector3.zero;
            m_playerChargeLight.enabled = true;
            m_playerChargeLight.intensity += m_lightIntensityPerFrame;
            if (m_playerChargeLight.intensity > m_maxLightIntensity)
            {
                m_playerChargeLight.intensity = m_maxLightIntensity;
            }
        }
        else if (m_playerChargeLight.isActiveAndEnabled)
        {
            m_playerChargeLight.intensity -= m_lightIntensityPerFrame;
            if (m_playerChargeLight.intensity < m_defaultLightIntensity)
            {
                m_playerChargeLight.intensity = m_defaultLightIntensity;
                m_playerChargeLight.enabled = false;
            }
        }
    }

    public void PerformRollOver()
    {
        m_roolOverActivated = true;
        //m_playerRb.isKinematic = true;
        m_playerRb.velocity = Vector3.zero;
        m_gameManager.m_abilityButton2.interactable = false;
        StartCoroutine(CooldownExecution());

        StartCoroutine(ExecutedRollOver());
    }

    IEnumerator ExecutedRollOver()
    {
        yield return new WaitForSeconds(m_rollOverDelay);
        //m_playerRb.isKinematic = false;
        m_roolOverActivated = false;
        Vector3 movementDirection = new Vector3(xAxis, 0, zAxis);
        m_playerRb.AddForce(movementDirection * m_force, ForceMode.VelocityChange);
    }

    IEnumerator CooldownExecution()
    {
        yield return new WaitForSeconds(m_cooldown);
        m_gameManager.m_abilityButton2.interactable = true;
    }
}
