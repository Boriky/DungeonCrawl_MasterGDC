using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Gameplay Values")]
    [SerializeField] int m_damage = 20;
    [SerializeField] float m_timer = 3.0f;

    [Header("Explosion Parameters")]
    [SerializeField] float m_force = 1.0f;
    [SerializeField] float m_radius = 1.0f;
    [SerializeField] float m_upwardsModifier = 2.0f;

    [Header("Explosion Effects")]
    [SerializeField] ParticleSystem m_explosionGFX;
    [SerializeField] AudioSource m_explosionSFX;

    private Rigidbody m_rigidBody = null;
    ParticleSystem[] m_particles = new ParticleSystem[8];

    ParticleSystem m_particlesRef = null;

    private void Start()
    {
        m_explosionGFX = GameObject.Find("ProjectileExplosion").GetComponent<ParticleSystem>();
        m_explosionSFX = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (m_particlesRef != null && !m_particlesRef.isPlaying)
        {
            Destroy(m_particlesRef.gameObject);
        }
    }

    public void ExecuteTimedExplosionCoroutine(GameObject i_projectile, ShootAround i_shootAbility, int ENEMY = -1)
    {
        StartCoroutine(TimedExplosion(i_projectile, i_shootAbility, ENEMY));
    }

    /// <summary>
    ///  Coroutine that generates an explosion at the projectile position, after a presetted timer, dealing damage to in range shields
    /// </summary>
    /// <param name="i_projectile"></param>
    /// <returns></returns>
    public IEnumerator TimedExplosion(GameObject i_projectile, ShootAround i_shootAbility, int ENEMY = -1)
    {
        yield return new WaitForSeconds(m_timer);

        Vector3 explosionPos = i_projectile.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, m_radius);

        m_particlesRef = Instantiate(m_explosionGFX, transform.position, Quaternion.identity);
        m_particlesRef.Play();
        m_explosionSFX.Play();

        foreach (Collider hit in colliders)
        {
            // Add an explosion force to in range rigidbodies
            Rigidbody hittedRb = hit.GetComponent<Rigidbody>();
            if (hittedRb != null)
            {
                hittedRb.AddExplosionForce(m_force, explosionPos, m_radius, m_upwardsModifier, ForceMode.Impulse);
            }

            // Damage the shield of in range enemies
            EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.DamageShield(m_damage);
            }

            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(m_damage / 2, Vector3.zero);
            }
        }

        if (i_shootAbility != null)
        {
            i_shootAbility.ResetBulletsAndStartAnimation();
            StartCoroutine(i_shootAbility.CooldownExecution());
        }

        if (ENEMY != -1)
        {
            Destroy(i_projectile);
        }
    }
}