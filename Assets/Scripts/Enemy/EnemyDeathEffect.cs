using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathEffect : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField]
    GameObject[] m_enemyParts = null;

    [Header("Explosion Parameters")]
    [SerializeField]
    float m_force = 1.0f;
    [SerializeField]
    float m_radius = 1.0f;
    [SerializeField]
    float m_upwardsModifier = 2.0f;

    [Header("Effects")]
    [SerializeField]
    ParticleSystem m_explosionGFX = null;

    private ParticleSystem m_particlesRef = null;

    private void Start()
    {
        m_explosionGFX = GameObject.Find("EnemyDeath").GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (m_particlesRef != null && !m_particlesRef.isPlaying)
        {
            Destroy(m_particlesRef.gameObject);
        }
    }

    public void EnemyDeathExplosion()
    {
        m_particlesRef = Instantiate(m_explosionGFX, transform.position, Quaternion.identity);
        m_particlesRef.Play();

            Vector3 explosionPos = transform.position;

            for (int index = 0; index < m_enemyParts.Length; ++index)
            {
                GameObject bodyPart = m_enemyParts[index];
                bodyPart.transform.parent = transform.root;
                Rigidbody bodyPartRb = bodyPart.GetComponent<Rigidbody>();
                bodyPartRb.isKinematic = false;
                bodyPartRb.useGravity = true;
                bodyPartRb.AddExplosionForce(m_force, explosionPos, m_radius, m_upwardsModifier, ForceMode.Impulse);

                Explosion bombExplosion = bodyPart.GetComponent<Explosion>();
                if (bombExplosion != null)
                {
                    bombExplosion.ExecuteTimedExplosionCoroutine(bodyPart, null, 0);
                }
            }
    }
}
