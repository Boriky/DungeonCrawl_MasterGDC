using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int m_startingHealth = 100;
    public int m_currentHealth;
    public float m_sinkSpeed = 2.5f;
    public int m_scoreValue = 10;

    private ParticleSystem hitParticles = null;
    private BoxCollider m_boxCollider = null;
    private bool isDead = false;
    private bool isSinking = false;

	// Use this for initialization
	void Start ()
    {
        hitParticles = GetComponent<ParticleSystem>();
        m_boxCollider = GetComponent<BoxCollider>();
        m_currentHealth = m_startingHealth;	
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (isSinking)
        {
            transform.Translate(-Vector3.up * m_sinkSpeed * Time.deltaTime);
        }
	}

    public void TakeDamage (int damageAmount)
    {
        if (isDead)
            return;

        m_currentHealth -= damageAmount;
        
        if(m_currentHealth <= 0)
        {
            isDead = true;
            m_boxCollider.isTrigger = true;
        }
    }
}
