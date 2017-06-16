using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int m_damagePerHit = 10;
    
    private int m_collidableMask = 0;
    ParticleSystem m_hitParticles = null;
    AudioSource m_hitAudio = null;

    void Awake ()
    {
        m_collidableMask = LayerMask.GetMask("Player");
        m_hitParticles = GetComponent<ParticleSystem>();
        m_hitAudio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;

        if (m_collidableMask == collisionObject.layer)
        {
            PlayerHealth playerHealth = collisionObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(m_damagePerHit);
            }
        }
    }
}
