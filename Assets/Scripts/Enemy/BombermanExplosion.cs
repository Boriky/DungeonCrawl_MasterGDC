using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Behaviour that defines the explosion of a kamikaze enemy
public class BombermanExplosion : MonoBehaviour
{
    [Header("Gameplay Values")]
    [SerializeField] int m_damageToPlayer = 20;
    [SerializeField] int m_damageToEnemies = 50;
    [SerializeField] int m_damageToSelf = 100;

    [Header("Explosion Parameters")]
    [SerializeField] float m_force = 1.0f;
    [SerializeField] float m_upwardsModifier = 2.0f;

    [Header("Effects")]
    [SerializeField] ParticleSystem m_explosionGFX = null;

    private SphereCollider m_explosionTrigger = null;
    private ParticleSystem m_explosionParticleRef = null;

    private void Start()
    {
        m_explosionTrigger = GetComponent<SphereCollider>();
        m_explosionGFX = GameObject.Find("BombermanExplosion").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // Destroy the explosion's particle when it's no longer playing
        if (m_explosionParticleRef != null && !m_explosionParticleRef.isPlaying)
        {
            Destroy(m_explosionParticleRef.gameObject);
        }
    }

    /// <summary>
    /// Activate the self destruction explosion when the player is in the range defined by the trigger
    /// </summary>
    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "Player")
        {
            ActivateSelfDestruction();
        }
    }

    /// <summary>
    /// Initialize and execute the self destruction procedure
    /// </summary>
    private void ActivateSelfDestruction()
    {
        ActivateParticle();

        ProcessExplosion();

        Suicide();
    }

    /// <summary>
    /// Instantiate and play the explosion's gfx particle at the enemy position
    /// </summary>
    void ActivateParticle()
    {
        m_explosionParticleRef = Instantiate(m_explosionGFX, transform.position, Quaternion.identity);
        m_explosionParticleRef.Play();
    }

    /// <summary>
    /// Collect all collidable objects, generate a physics explosion effect and damage in range player and enemies
    /// </summary>
    void ProcessExplosion()
    {
        // Collect all the collidable objects in the explosion radius (enemies and physics objects)
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
                enemyHealth.DamageShield(m_damageToEnemies);
            }

            // Damage the player
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(m_damageToPlayer, Vector3.zero);
            }

        }
    }

    /// <summary>
    /// Kill the bomberman!
    /// </summary>
    void Suicide()
    {
        Transform parent = transform.parent;
        parent.GetComponent<EnemyHealth>().TakeDamage(m_damageToSelf);
    }
}
