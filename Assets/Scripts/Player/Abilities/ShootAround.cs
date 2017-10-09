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
    [SerializeField] float[] bulletsYRotations;

    private int m_numberOfBullets;
    private GameManager m_gameManager = null;

    // Use this for initialization
    void Awake ()
    {
        m_numberOfBullets = bulletsPositions.Length;
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        for (int index = 0; index < m_numberOfBullets; ++index)
        {
            GameObject projInstance = Instantiate(m_projectile, transform.position + bulletsPositions[index], Quaternion.Euler(Vector3.up * bulletsYRotations[index]));
            Transform projTransform = projInstance.transform;
            projTransform.parent = GameObject.Find("Room(Clone)").transform;

            Rigidbody projRb = projTransform.GetComponent<Rigidbody>();
            projRb.AddForce(projRb.transform.up * m_bulletVelocity / 3.0f, ForceMode.Impulse);
            projRb.AddForce(projRb.transform.forward * m_bulletVelocity, ForceMode.Impulse);

            Explosion explosion = projInstance.GetComponent<Explosion>();
            StartCoroutine(explosion.TimedExplosion(projInstance));

            m_gameManager.m_abilityButton4.interactable = false;
            StartCoroutine(CooldownExecution());
        } 
    }

    IEnumerator CooldownExecution()
    {
        yield return new WaitForSeconds(m_cooldown);
        m_gameManager.m_abilityButton4.interactable = true;
    }
}
