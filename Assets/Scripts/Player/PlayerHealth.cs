using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnPlayerDeathDelegate(PlayerHealth i_Listener);

public class PlayerHealth : MonoBehaviour
{
    [Header("Gameplay values")]
    [SerializeField] int m_startingHealth = 100;
    [SerializeField] int m_currentHealth = 0;
    [SerializeField] float m_minSpotAngle = 5.0f;
    [SerializeField] float m_maxSpotAngle = 30.0f;
    [SerializeField] Light m_directLight = null;
    [SerializeField] int m_healthRegeneration = 10;

    [Header("Damage indicators")]
    [SerializeField] float m_flashSpeed = 5f;
    [SerializeField] Color m_flashColor = new Color(1f, 0f, 0f, 0.1f);

    [Header("Death indicator")]
    [SerializeField] AudioClip m_deathClip = null;

    // death delegate for game manager
    public OnPlayerDeathDelegate m_onDeathEvent = null;

    private bool m_isDamageable = true;
    private bool m_isDead = false;
    private bool m_damaged = false;
    private GameManager m_gameManager = null;
    private Material m_playerMaterial = null;
    private Rigidbody m_rigidBody = null;
    private AudioSource m_playerAudio = null;
    private Slider m_healthBar = null;

    // Use this for initialization
    void Awake ()
    {
        m_currentHealth = m_startingHealth;
        m_playerMaterial = GetComponent<Renderer>().material;
        m_rigidBody = GetComponent<Rigidbody>();
        m_playerAudio = GetComponent<AudioSource>();
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_healthBar = m_gameManager.GetPlayerHealthBar();
        m_directLight = GameObject.Find("Spotlight").GetComponent<Light>();
        m_maxSpotAngle = m_directLight.spotAngle;
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_directLight.spotAngle = Random.Range(m_directLight.spotAngle - 0.1f, m_directLight.spotAngle + 0.1f);

        if (m_damaged)
        {
            m_playerMaterial.color = m_flashColor;
        }
        else
        {
            m_playerMaterial.color = Color.Lerp(m_playerMaterial.color, Color.clear, m_flashSpeed * Time.deltaTime);
        }
        m_damaged = false;

        m_healthBar.value = m_currentHealth;
    }

    /// <summary>
    /// Player takes damage for damageAmount; if health drops below zero, death event is called
    /// </summary>
    /// <param name="i_damageAmount"></param>
    public void TakeDamage (int i_damageAmount)
    {
        if (m_isDamageable)
        {
            m_damaged = true;

            m_currentHealth -= i_damageAmount;
            m_healthBar.value = m_currentHealth;

            if (m_directLight.spotAngle > m_minSpotAngle)
            {
                m_directLight.spotAngle -= i_damageAmount / 2;
            }

            if (m_currentHealth <= 0 && !m_isDead)
            {
                Death();
            }
        }
    }

    /// <summary>
    /// Manages player death event
    /// </summary>
    void Death ()
    {
        m_isDead = true;

        m_playerAudio.clip = m_deathClip;
        m_playerAudio.Play();

        m_rigidBody.isKinematic = true;

        m_onDeathEvent(this);
    }

    /// <summary>
    /// Restore some of the player health when he enters inside the level's lights trigger
    /// </summary>
    private void OnTriggerEnter(Collider col)
    {
        SlowlyDisableLight disableLight = col.GetComponent<SlowlyDisableLight>();
        if (disableLight != null && !disableLight.m_deactivating)
        {
            m_currentHealth += m_healthRegeneration;
            if (m_currentHealth > m_startingHealth)
            {
                m_currentHealth = m_startingHealth;
            }
            m_healthBar.value = m_currentHealth;

            m_directLight.spotAngle += m_healthRegeneration / 2;
            if (m_directLight.spotAngle > m_maxSpotAngle)
            {
                m_directLight.spotAngle = m_maxSpotAngle;
            }

            disableLight.m_deactivating = true;
        }
    }
}
