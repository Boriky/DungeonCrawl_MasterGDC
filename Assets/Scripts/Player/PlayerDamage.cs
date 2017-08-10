using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [Header("Gameplay values")]
    [SerializeField] int m_damagePerHit = 20;
    //public List<Ability> m_activePowerups = null;

    private int m_collidableMask = 0;
    private ParticleSystem m_hitParticles = null;
    private AudioSource m_hitAudio = null;

	// Use this for initialization
	void Awake ()
    {
        m_collidableMask = LayerMask.GetMask("Enemy");
        m_hitParticles = GetComponent<ParticleSystem>();
        m_hitAudio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision col)
    {
        GameObject hit = col.gameObject;

        if ("Enemy" == hit.tag)
        {
            // TopCollider aka "Enemy" tag is child of BasicEnemy aka "Shield" tag
            EnemyHealth enemyHealth = hit.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(m_damagePerHit);
            }
        }
    }
}
