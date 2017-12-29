using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAround : Ability
{
    [Header("Prefabs")]
    [SerializeField] GameObject m_projectile = null;

    [Header("Gameplay Values")]
    [SerializeField] float m_bulletVelocity = 10.0f;
    [SerializeField] float m_cooldown = 2.0f;

    [Header("Starting Positions")]
    [SerializeField] Vector3[] bulletsPositions;
    [SerializeField] Quaternion[] bulletsYRotations;

    private int m_numberOfBullets;
    private GameManager m_gameManager = null;
    private Transform[] m_projectilesInstances = null;
    private Animator m_projectileAnimator = null;
    private AudioSource[] m_playerSFXs = null;

    // Use this for initialization
    void Awake ()
    {
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_playerSFXs = GetComponents<AudioSource>();
    }

    private void Start()
    {
        m_projectile = GameObject.Find("Projectiles");
        m_projectileAnimator = m_projectile.GetComponent<Animator>();

        m_numberOfBullets = m_projectile.transform.childCount;

        m_projectilesInstances = new Transform[m_numberOfBullets];
        for (int index = 0; index < m_numberOfBullets; ++index)
        {
            m_projectilesInstances[index] = m_projectile.transform.GetChild(index);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.R))
        {
            FireProjectiles();
        }
	}

    /// <summary>
    /// Instantiate "m_numberOfBullets" projectiles as children of "Room", force them a velocity and start the TimedExplosion coroutine on each projectile
    /// </summary>
    public void FireProjectiles()
    {
        m_playerSFXs[4].Play();
        for (int index = 0; index < m_numberOfBullets; ++index)
        {
            GameObject projInstance = m_projectilesInstances[index].gameObject;

            projInstance.GetComponent<SphereCollider>().isTrigger = false;
            bulletsPositions[index] = projInstance.transform.localPosition;
            bulletsYRotations[index] = projInstance.transform.localRotation;
            projInstance.transform.parent = transform.root;

            Rigidbody projRb = projInstance.GetComponent<Rigidbody>();
            projRb.isKinematic = false;
            projRb.AddForce(projInstance.transform.up * m_bulletVelocity / 3.0f, ForceMode.Impulse);
            projRb.AddForce(projInstance.transform.forward * m_bulletVelocity, ForceMode.Impulse);

            Explosion explosion = projInstance.GetComponent<Explosion>();
            if (index == m_numberOfBullets - 1)
            {
                explosion.ExecuteTimedExplosionCoroutine(projInstance, this);

            }
            else
            {
                explosion.ExecuteTimedExplosionCoroutine(projInstance, null);
            }
 
            m_gameManager.m_abilityButton4.interactable = false;
            //StartCoroutine(CooldownExecution());
        }
        m_projectile.transform.localScale = new Vector3(0.0f,0.0f,0.0f);
    }

    public IEnumerator CooldownExecution()
    {
        yield return new WaitForSeconds(m_cooldown);

        m_projectileAnimator.SetBool("CDActive", false);
        m_gameManager.m_abilityButton4.interactable = true;
    }

    public void ResetBulletsAndStartAnimation()
    {
        m_projectileAnimator.SetBool("CDActive", true);

        for (int index = 0; index < m_projectilesInstances.Length; ++index)
        {
            Transform projInstance = m_projectilesInstances[index];
            projInstance.gameObject.GetComponent<SphereCollider>().isTrigger = true;
            projInstance.parent = m_projectile.transform;
            projInstance.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            projInstance.localPosition = bulletsPositions[index];
            projInstance.localRotation = bulletsYRotations[index];
        }

    }
}
