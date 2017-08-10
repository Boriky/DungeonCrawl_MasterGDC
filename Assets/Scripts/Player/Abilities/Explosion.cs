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

    /// <summary>
    ///  Coroutine that generates an explosion at the projectile position, after a presetted timer, dealing damage to in range shields
    /// </summary>
    /// <param name="i_projectile"></param>
    /// <returns></returns>
    public IEnumerator TimedExplosion(GameObject i_projectile)
    {
        yield return new WaitForSeconds(m_timer);

        Vector3 explosionPos = i_projectile.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, m_radius);

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
        }

        Destroy(i_projectile);
    }
}