using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnDeathDelegate(EnemyHealth i_listener, int i_enemyID);
public delegate void OnLifeChangedDelegate(float i_prev, float i_new);

public class EnemyHealth : MonoBehaviour
{
    [Header("Gameplay values")]
    [SerializeField] int m_startingHealth = 100;
    [SerializeField] int m_currentHealth;
    [SerializeField] int m_startingShield = 100;
    [SerializeField] int m_currentShield;
    [SerializeField] float m_sinkSpeed = 2.5f;
    [SerializeField] int m_scoreValue = 10;
    [SerializeField] float m_repulsiveForceWhenDamaged = 5.0f;
    [SerializeField] bool m_isBomberman = false;

    [Header("User interface")]
    [SerializeField] Slider m_healthBar = null;
    [SerializeField] Slider m_shieldBar = null;

    public OnDeathDelegate m_onDeathEvent = null;
    public OnLifeChangedDelegate m_onLifeChangedEvent = null;

    private ParticleSystem hitParticles = null;
    private BoxCollider m_boxCollider = null;
    public bool isDead = false;
    private bool isSinking = false;
    private EnemyDeathEffect m_enemyDeathExplosion = null;
    private int m_enemyID;

	// Use this for initialization
	void Awake ()
    {
        hitParticles = GetComponent<ParticleSystem>();
        m_boxCollider = GetComponent<BoxCollider>();
        m_currentHealth = m_startingHealth;
        m_currentShield = m_startingShield;
        m_enemyDeathExplosion = GetComponent<EnemyDeathEffect>();
        m_enemyID = GetComponent<Enemy>().m_enemyID;
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
    public void TakeDamage (int i_damageAmount, Rigidbody i_playerRb = null)
    {
        if (isDead)
            return;

        m_currentHealth -= i_damageAmount;
        m_healthBar.value = m_currentHealth;
        
        if(m_currentHealth <= 0)
        {
            isDead = true;
            isSinking = true;
            m_boxCollider.isTrigger = true;
            m_enemyDeathExplosion.EnemyDeathExplosion();
            m_onDeathEvent(this, m_enemyID);
        }
        else
        {
            if (i_playerRb != null)
            {
                i_playerRb.AddForce(-gameObject.transform.forward * m_repulsiveForceWhenDamaged, ForceMode.Impulse);
            }
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
        m_shieldBar.value = m_currentShield;

        if (m_isBomberman)
        {
            TakeDamage(i_damageAmount);
        }

        if (m_currentShield <= 0)
        {
            // TODO just change the color instead of swapping material!
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            Material newMaterial = new Material(Shader.Find("Legacy Shaders/VertexLit"));
            newMaterial.color = Color.green;
            meshRenderer.material = newMaterial;

            // make so that the shield now becomes a damageable part
            tag = "Enemy";
        }
    }
}
