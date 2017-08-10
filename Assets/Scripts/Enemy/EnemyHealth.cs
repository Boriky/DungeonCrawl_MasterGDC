using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnDeathDelegate(EnemyHealth i_Listener);
public delegate void OnLifeChangedDelegate(float i_Prev, float i_New);

public class EnemyHealth : MonoBehaviour
{
    [Header("Gameplay values")]
    [SerializeField] int m_startingHealth = 100;
    [SerializeField] int m_currentHealth;
    [SerializeField] int m_startingShield = 100;
    [SerializeField] int m_currentShield;
    [SerializeField] float m_sinkSpeed = 2.5f;
    [SerializeField] int m_scoreValue = 10;

    public OnDeathDelegate m_onDeathEvent = null;
    public OnLifeChangedDelegate m_onLifeChangedEvent = null;

    private ParticleSystem hitParticles = null;
    private BoxCollider m_boxCollider = null;
    private bool isDead = false;
    private bool isSinking = false;

	// Use this for initialization
	void Awake ()
    {
        hitParticles = GetComponent<ParticleSystem>();
        m_boxCollider = GetComponent<BoxCollider>();
        m_currentHealth = m_startingHealth;
        m_currentShield = m_startingShield;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (isSinking)
        {
            transform.Translate(-Vector3.up * m_sinkSpeed * Time.deltaTime);
        }
	}

    /// <summary>
    /// Enemy takes damage for damageAmount; if health drops below zero, death event is called
    /// </summary>
    /// <param name="i_damageAmount"></param>
    public void TakeDamage (int i_damageAmount)
    {
        if (isDead)
            return;

        m_currentHealth -= i_damageAmount;
        
        if(m_currentHealth <= 0)
        {
            isDead = true;
            isSinking = true;
            m_boxCollider.isTrigger = true;
            m_onDeathEvent(this);
        }
    }

    /// <summary>
    /// Shield takes damage for damageAmount; if shield drops below zero, shield gets destroyed
    /// </summary>
    /// <param name="i_damageAmount"></param>
    public void DamageShield (int i_damageAmount)
    {
        if (isDead)
            return;

        m_currentShield -= i_damageAmount;

        if (m_currentShield <= 0)
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            Material newMaterial = new Material(Shader.Find("Specular"));
            newMaterial.color = Color.green;
            meshRenderer.material = newMaterial;
        }
    }
}
