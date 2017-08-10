using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAround : Ability
{
    [Header("Prefabs")]
    [SerializeField] GameObject m_projectile = null;

    [Header("Gameplay Values")]
    [SerializeField] float m_bulletVelocity = 10.0f;

    [Header("Starting Positions")]
    [SerializeField] Vector3[] bulletsPositions;
    [SerializeField] float[] bulletsYRotations;

    private int m_numberOfBullets;

	// Use this for initialization
	void Start ()
    {
        m_numberOfBullets = bulletsPositions.Length;
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
    void FireProjectiles()
    {
        for (int index = 0; index < m_numberOfBullets; ++index)
        {
            GameObject projInstance = Instantiate(m_projectile, transform.position + bulletsPositions[index], Quaternion.Euler(Vector3.up * bulletsYRotations[index]));
            Transform projTransform = projInstance.transform;
            projTransform.parent = GameObject.Find("Room(Clone)").transform;

            Rigidbody projRb = projTransform.GetComponent<Rigidbody>();
            projRb.velocity = projRb.transform.forward * m_bulletVelocity;

            Explosion explosion = projInstance.GetComponent<Explosion>();
            StartCoroutine(explosion.TimedExplosion(projInstance));
        } 
    }
}
