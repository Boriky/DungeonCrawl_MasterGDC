using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [Header("Gameplay values")]
    [SerializeField] int m_damagePerHit = 20;
    [SerializeField] float m_playerVelocityThreshold = 10.0f;

    private AudioSource m_hitAudio = null;
    private Rigidbody m_playerRb = null;

	// Use this for initialization
	void Awake ()
    {
        m_hitAudio = GetComponent<AudioSource>();
        m_playerRb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision col)
    {
        GameObject hit = col.gameObject;

        if ("Enemy" == hit.tag)
        {
            // TopCollider aka "Enemy" tag is child of BasicEnemy aka "Shield" tag
            EnemyHealth enemyHealth = hit.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null && m_playerRb.velocity.magnitude > m_playerVelocityThreshold)
            {
                enemyHealth.TakeDamage(m_damagePerHit, m_playerRb);
            }
        }
    }
}
