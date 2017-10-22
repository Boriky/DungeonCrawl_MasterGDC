using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mines : MonoBehaviour
{
    [Header("Gameplay Values")]
    [SerializeField]
    int m_damage = 20;
    [SerializeField]
    float m_timer = 3.0f;

    [Header("Explosion Parameters")]
    [SerializeField]
    float m_force = 1.0f;
    [SerializeField]
    float m_radius = 1.0f;
    [SerializeField]
    float m_upwardsModifier = 2.0f;

    [Header("Explosion Effects")]
    [SerializeField]
    ParticleSystem m_explosionGFX;
    [SerializeField]
    AudioSource m_explosionSFX;

    private bool isMineActivated = false;
    private Rigidbody m_rigidBody = null;

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    public IEnumerator ActivateMine()
    {
        yield return new WaitForSeconds(m_timer);
        isMineActivated = true;
    }

    private void OnCollisionEnter(Collision col)
    {
        string colTag = col.gameObject.tag;

        if (!isMineActivated && colTag == "Floor")
        {
            m_rigidBody.isKinematic = true;
            StartCoroutine(ActivateMine());
        }

        if (isMineActivated)
        {
            Vector3 explosionPos = transform.position;
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

                PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(m_damage / 2, Vector3.zero);
                }
            }

            Destroy(gameObject);
        }
    }
}
