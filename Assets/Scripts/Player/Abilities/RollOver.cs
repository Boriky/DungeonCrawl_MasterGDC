using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("UI References")]
    [SerializeField] Image m_spriteDirectionIndicator = null;

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
        m_playerChargeLight = GameObject.Find("PlayerChargeLight").GetComponent<Light>();
    }

    private void Start()
    {
        m_spriteDirectionIndicator = GameObject.Find("ArrowIndicator").GetComponent<Image>();
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
            m_spriteDirectionIndicator.enabled = true;

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
            m_spriteDirectionIndicator.enabled = false;

            m_playerChargeLight.intensity -= m_lightIntensityPerFrame;
            if (m_playerChargeLight.intensity < m_defaultLightIntensity)
            {
                m_playerChargeLight.intensity = m_defaultLightIntensity;
                m_playerChargeLight.enabled = false;
            }
        }

        RotateSpriteDirectionIndicator();
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

    private void RotateSpriteDirectionIndicator()
    {
        m_spriteDirectionIndicator.transform.parent.position = transform.position;

        Vector3 movementDirection = new Vector3(-xAxis, 0, -zAxis);
        Vector3 spriteDirectionForward = m_spriteDirectionIndicator.transform.parent.forward;
        float step = 10.0f * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(spriteDirectionForward, movementDirection, step, 0.0f);
        m_spriteDirectionIndicator.transform.parent.rotation = Quaternion.LookRotation(newDirection.normalized);
    }

    IEnumerator ExecutedRollOver()
    {
        yield return new WaitForSeconds(m_rollOverDelay);
        //m_playerRb.isKinematic = false;
        m_roolOverActivated = false;
        Vector3 movementDirection = new Vector3(xAxis, 0, zAxis);
        m_playerRb.AddForce(movementDirection.normalized * m_force, ForceMode.VelocityChange);
    }

    IEnumerator CooldownExecution()
    {
        yield return new WaitForSeconds(m_cooldown);
        m_gameManager.m_abilityButton2.interactable = true;
    }
}
