using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollOver : Ability
{
    [Header("Gameplay values")]
    [SerializeField] float m_force = 5.0f;
    [SerializeField] bool m_keyboardControls = true;
    [SerializeField]
    float m_cooldown = 1.0f;

    private Rigidbody m_playerRb = null;
    private GameManager m_gameManager = null;

    float zAxis;
    float xAxis;

    // Use this for initialization
    void Awake ()
    {
        m_playerRb = GetComponent<Rigidbody>();
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update ()
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            PerformRollOver();
        }
    }

    public void PerformRollOver()
    {
        Vector3 movementDirection = new Vector3(xAxis, 0, zAxis);
        m_playerRb.AddForce(movementDirection * m_force, ForceMode.VelocityChange);

        m_gameManager.m_abilityButton2.interactable = false;
        StartCoroutine(CooldownExecution());
    }

    IEnumerator CooldownExecution()
    {
        yield return new WaitForSeconds(m_cooldown);
        m_gameManager.m_abilityButton2.interactable = true;
    }
}
