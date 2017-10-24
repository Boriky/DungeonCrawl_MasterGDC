using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Gameplay values")]
    [SerializeField] int m_damagePerHit = 10;
    
    private ParticleSystem m_hitParticles = null;
    private AudioSource m_hitAudio = null;

    void Awake ()
    {
        m_hitParticles = GetComponent<ParticleSystem>();
        m_hitAudio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Enemy damages the player on collision and pass the contact point between the two
    /// </summary>
    void OnCollisionEnter(Collision col)
    {
        GameObject hit = col.gameObject;

        if ("Player" == hit.tag && tag != "Enemy")
        {
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(m_damagePerHit, col.contacts[0].point);
            }
        }
    }
}
