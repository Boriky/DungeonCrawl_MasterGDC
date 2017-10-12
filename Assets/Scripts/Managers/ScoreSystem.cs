using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    [Header("Gameplay values")]
    [SerializeField] float m_baseEnemyBonus = 15.0f;
    [SerializeField] float m_spikeEnemyBonus = 15.0f;
    [SerializeField] float m_bombsEnemyBonus = 15.0f;
    [SerializeField]float m_playerHealthBonus = 15.0f;
    [SerializeField] float m_malusPerSecond = 15.0f;

    [Header("UI References")]
    [SerializeField] Text m_timeText = null;

    private float m_score;
    private float m_time;
    private bool m_isGameActive = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (m_isGameActive)
        {
            m_time += Time.deltaTime;
            m_timeText.text = m_time.ToString();
        }
        else
        {
            // detract a malus for every second the game has been going on
            m_score += m_time * m_malusPerSecond + (m_playerHealthBonus /* * everyHPPlayerHasLeft */ );
        }
	}

    public void SetGameAsActive(bool i_isActive)
    {
        m_isGameActive = i_isActive;
    }
}
