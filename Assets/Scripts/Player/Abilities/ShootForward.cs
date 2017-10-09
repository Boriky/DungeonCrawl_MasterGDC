using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootForward : Ability
{
    [Header("Prefabs")]
    [SerializeField] GameObject m_projectile = null;

    [Header("Gameplay Values")]
    [SerializeField] float m_bulletVelocity = 10.0f;
    [SerializeField] bool m_keyboardControls = true;
    [SerializeField] float m_cooldown = 1.5f;

    private float zAxis = 0.0f;
    private float xAxis = 0.0f;
    private GameManager m_gameManager = null;

    private void Awake()
    {
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    } 

    // Update is called once per frame
    void Update ()
    {
        SetAxis();

        if ((zAxis != 0 || xAxis != 0) && Input.GetKeyDown(KeyCode.F))
        {
            FireProjectile();
        }
    }

    /// <summary>
    /// Set input axis
    /// </summary>
    void SetAxis()
    {
        if (m_keyboardControls)
        {
            zAxis = Input.GetAxis("Vertical");
            xAxis = Input.GetAxis("Horizontal");
        }
        else
        {
            zAxis = (Input.acceleration.y /*- m_zStart*/);
            xAxis = (Input.acceleration.x /*- m_xStart*/);
        }
    }

    /// <summary>
    /// Instantiate a projectile as children of "Room", force a velocity on the projectile and start the TimedExplosion coroutine on it
    /// </summary>
    /// <param name="i_zAxis"></param>
    /// <param name="i_xAxis"></param>
    public void FireProjectile()
    {
        Vector3 movementDirection = new Vector3(xAxis, 0, zAxis);

        float zOffset = 1.0f;
        float xOffset = 1.0f;

        if (zAxis > 0)
        {
            zOffset = -1;
        }
        else if (zAxis < 0)
        {
            zOffset = 1;
        }

        if (xAxis > 0)
        {
            xOffset = -1;
        }
        else if (xAxis < 0) 
        {
            xOffset = 1;
        }

        Vector3 spawnLocation = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset);

        GameObject projInstance = Instantiate(m_projectile, spawnLocation, Quaternion.identity);
        Transform projTransform = projInstance.transform;
        projTransform.parent = GameObject.Find("Room(Clone)").transform;

        Rigidbody projRb = projInstance.GetComponent<Rigidbody>();
        projRb.velocity = -movementDirection * m_bulletVelocity;

        Explosion explosion = projInstance.GetComponent<Explosion>();
        StartCoroutine(explosion.TimedExplosion(projInstance));

        m_gameManager.m_abilityButton3.interactable = false;
        StartCoroutine(CooldownExecution());
    }

    IEnumerator CooldownExecution()
    {
        yield return new WaitForSeconds(m_cooldown);
        m_gameManager.m_abilityButton3.interactable = true;
    }
}
