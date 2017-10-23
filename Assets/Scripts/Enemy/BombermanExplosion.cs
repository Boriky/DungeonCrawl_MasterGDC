using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombermanExplosion : MonoBehaviour
{
    [Header("Gameplay Values")]
    [SerializeField]
    int m_damage = 20;

    [Header("Explosion Parameters")]
    [SerializeField]
    float m_force = 1.0f;
    [SerializeField]
    float m_upwardsModifier = 2.0f;

    [Header("Effects")]
    [SerializeField]
    ParticleSystem m_explosionGFX = null;

    private SphereCollider m_explosionTrigger = null;
    ParticleSystem m_explosionParticleRef = null;

    private void Start()
    {
        m_explosionTrigger = GetComponent<SphereCollider>();
        m_explosionGFX = GameObject.Find("BombermanExplosion").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (m_explosionParticleRef != null && !m_explosionParticleRef.isPlaying)
        {
            Destroy(m_explosionParticleRef.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ActivateSelfDestruction();
        }
    }

    private void ActivateSelfDestruction()
    {
        m_explosionParticleRef = Instantiate(m_explosionGFX, transform.position, Quaternion.identity);
        m_explosionParticleRef.Play();

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, m_explosionTrigger.radius);

        foreach (Collider hit in colliders)
        {
            // Add an explosion force to in range rigidbodies
            Rigidbody hittedRb = hit.GetComponent<Rigidbody>();
            if (hittedRb != null)
            {
                hittedRb.AddExplosionForce(m_force, explosionPos, m_explosionTrigger.radius, m_upwardsModifier, ForceMode.Impulse);
            }

            // Damage the shield of in range enemies
            EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.DamageShield(m_damage / 2);
            }

            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(m_damage, Vector3.zero);
            }
        }

        Transform parent = transform.parent;
        parent.GetComponent<EnemyHealth>().TakeDamage(100);
    }
}
