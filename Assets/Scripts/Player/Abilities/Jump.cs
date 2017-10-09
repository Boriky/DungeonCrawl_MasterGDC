using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jump : Ability
{
    [Header("Gameplay values")]
    [SerializeField] float m_force = 5.0f;
    [SerializeField] float m_force2 = 20.0f;
    [SerializeField] float m_forceAnticipation = 0.5f;
    [SerializeField] float m_smashDownDelay = 0.5f;
    [SerializeField] float m_rayLenght = 0.6f;
    [SerializeField] float m_cooldown = 0.5f;

    private Rigidbody m_playerRb = null;
    private static Vector3 s_rayDirection = Vector3.down;
    private bool m_isSmashJumpReady = false;
    private GameManager m_gameManager = null;

    void Awake()
    {
        m_playerRb = GetComponent<Rigidbody>();
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformJump();
        }
    }

    public void PerformJump()
    {
        // Stores everything the raycast is hitting
        RaycastHit hit;
        // Check if the raycast intersects with anything in its lenght
        if (Physics.Raycast(transform.position, s_rayDirection, out hit, m_rayLenght))
        {
            m_isSmashJumpReady = false;
            // Check if the raycast is hitting the floor and execute the jump command
            if (hit.collider.tag == "Floor")
            {
                m_playerRb.AddForce(Vector3.up * m_force, ForceMode.Impulse);
                m_isSmashJumpReady = true;
            }
        }
        else
        {
            // Double jump: after the first jump the player can jump again and perform a smash down towards the enemy
            if (m_isSmashJumpReady)
            {
                m_playerRb.velocity = Vector3.zero;
                m_playerRb.AddForce(Vector3.up * m_forceAnticipation, ForceMode.Impulse);

                m_gameManager.m_abilityButton1.interactable = false;
                StartCoroutine(CooldownExecution());

                StartCoroutine(PerformSmashDown());
            }
        }
    }

    IEnumerator PerformSmashDown()
    {
        yield return new WaitForSeconds(m_smashDownDelay);
        m_playerRb.AddForce(Vector3.down * m_force2, ForceMode.Impulse);
        /*m_gameManager.m_abilityButton1.interactable = false;

        m_gameManager.m_abilityButton1.interactable = false;
        StartCoroutine(CooldownExecution());*/
    }

    IEnumerator CooldownExecution()
    {
        yield return new WaitForSeconds(m_cooldown);
        m_gameManager.m_abilityButton1.interactable = true;
    }

}


// Update is called once per frame
/*void FixedUpdate()
{
    // Stores everything the raycast is hitting
    RaycastHit hit;
    // Check if the raycast intersects with anything in its lenght
    if (Physics.Raycast(transform.position, s_rayDirection, out hit, m_rayLenght))
    {
        m_isSmashJumpReady = false;
        // Check if the raycast is hitting the floor and execute the jump command
        if (hit.collider.tag == "Floor" && Input.GetKeyDown(KeyCode.Space))
        {
            m_playerRb.AddForce(Vector3.up * m_force, ForceMode.Impulse);
            m_isSmashJumpReady = true;
        }
    }
    else
    {
        // Double jump: after the first jump the player can jump again and perform a smash down towards the enemy
        if (m_isSmashJumpReady && Input.GetKeyDown(KeyCode.Space))
        {
            m_playerRb.velocity = Vector3.zero;
            m_playerRb.AddForce(Vector3.down * m_force2, ForceMode.Impulse);
        }
    }
}*/
