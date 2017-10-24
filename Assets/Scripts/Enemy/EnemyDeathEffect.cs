using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Behaviour that activates a death graphic effect when an enemy dies
public class EnemyDeathEffect : MonoBehaviour
{
    [Header("Physics Objects References")]
    [SerializeField] GameObject[] m_enemyParts = null;

    [Header("Explosion Parameters")]
    [SerializeField] float m_force = 1.0f;
    [SerializeField] float m_radius = 1.0f;
    [SerializeField] float m_upwardsModifier = 2.0f;

    [Header("Graphic Effects")]
    [SerializeField] ParticleSystem m_explosionGFX = null;

    private ParticleSystem m_particlesRef = null;

    void Start()
    {
        m_explosionGFX = GameObject.Find("EnemyDeath").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // Destroy the death effect's particle when it's no longer playing
        if (m_particlesRef != null && !m_particlesRef.isPlaying)
        {
            Destroy(m_particlesRef.gameObject);
        }
    }

    /// <summary>
    /// Execute the enemy death's effect
    /// </summary>
    public void EnemyDeathExplosion()
    {
        ActivateParticle();

        ProcessDeathEffect();
    }

    /// <summary>
    /// Instantiate and play the death effect's gfx particle at the enemy position
    /// </summary>
    void ActivateParticle()
    {
        m_particlesRef = Instantiate(m_explosionGFX, transform.position, Quaternion.identity);
        m_particlesRef.Play();
    }

    /// <summary>
    /// Add some velocity to the physics bodyparts of the enemy and execute a timed explosion if there are bombs attached
    /// </summary>
    void ProcessDeathEffect()
    {
        Vector3 explosionPos = transform.position;

        for (int index = 0; index < m_enemyParts.Length; ++index)
        {
            GameObject bodyPart = m_enemyParts[index];
            bodyPart.transform.parent = transform.root;

            Rigidbody bodyPartRb = bodyPart.GetComponent<Rigidbody>();
            bodyPartRb.isKinematic = false;
            bodyPartRb.useGravity = true;
            bodyPartRb.AddExplosionForce(m_force, explosionPos, m_radius, m_upwardsModifier, ForceMode.Impulse);

            ExecuteTimeExplosion(bodyPart);
        }
    }

    /// <summary>
    /// If the body part is a bomb, execute a timed explosion
    /// </summary>
    void ExecuteTimeExplosion(GameObject bomb)
    {
        Explosion bombExplosion = bomb.GetComponent<Explosion>();
        if (bombExplosion != null)
        {
            bombExplosion.ExecuteTimedExplosionCoroutine(bomb, null, 0);
        }
    }
}
