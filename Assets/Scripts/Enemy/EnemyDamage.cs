using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Gameplay values")]
    [SerializeField] int m_damagePerHit = 10;
    
    private int m_collidableMask = 0;
    private ParticleSystem m_hitParticles = null;
    private AudioSource m_hitAudio = null;

    void Awake ()
    {
        m_collidableMask = LayerMask.GetMask("Player");
        m_hitParticles = GetComponent<ParticleSystem>();
        m_hitAudio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision col)
    {
        GameObject hit = col.gameObject;

        if ("Player" == hit.tag && tag != "Enemy")
        {
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(m_damagePerHit);
            }
        }
    }
}
