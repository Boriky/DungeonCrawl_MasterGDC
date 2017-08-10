using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnPlayerDeathDelegate(PlayerHealth i_Listener);

public class PlayerHealth : MonoBehaviour
{
    [Header("Gameplay values")]
    [SerializeField] int m_startingHealth = 100;
    [SerializeField] int m_currentHealth = 0;

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
    private Material m_playerMaterial = null;
    private Rigidbody m_rigidBody = null;
    private AudioSource m_playerAudio = null;

    // Use this for initialization
    void Awake ()
    {
        m_currentHealth = m_startingHealth;
        m_playerMaterial = GetComponent<Renderer>().material;
        m_rigidBody = GetComponent<Rigidbody>();
        m_playerAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (m_damaged)
        {
            m_playerMaterial.color = m_flashColor;
        }
        else
        {
            m_playerMaterial.color = Color.Lerp(m_playerMaterial.color, Color.clear, m_flashSpeed * Time.deltaTime);
        }
        m_damaged = false;
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
}
