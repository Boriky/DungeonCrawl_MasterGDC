using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int m_startingHealth = 100;
    public int m_currentHealth;
    public float m_flashSpeed = 5f;
    public Color m_flashColor = new Color(1f, 0f, 0f, 0.1f);

    private bool m_isDead = false;
    private bool m_damaged = false;
    private Material m_playerMaterial = null;
	// Use this for initialization
	void Awake ()
    {
        m_playerMaterial = GetComponent<Material>();
        m_currentHealth = m_startingHealth;
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

    public void TakeDamage(int damageAmount)
    {
        m_damaged = true;

        m_currentHealth -= damageAmount;

        if (m_currentHealth <= 0 && !m_isDead)
        {
            m_isDead = true;
        }
    }
}
