using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public int m_damagePerHit = 20;
    //public List<Ability> m_activePowerups = null;

    private int m_collidableMask = 0;
    ParticleSystem m_hitParticles = null;
    AudioSource m_hitAudio = null;

	// Use this for initialization
	void Awake ()
    {
        m_collidableMask = LayerMask.GetMask("Enemy");
        m_hitParticles = GetComponent<ParticleSystem>();
        m_hitAudio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    	
	}

    void OnCollisionEnter(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;

        if ("Enemy" == collisionObject.tag)
        {
            EnemyHealth enemyHealth = collisionObject.GetComponentInParent<EnemyHealth>();
            Debug.Log("Hitted");
            if (enemyHealth != null)
            {
                Debug.Log("TakeDamage");
                enemyHealth.TakeDamage(m_damagePerHit);
            }
        }
    }
}
