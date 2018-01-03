using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    [Header("Gameplay values")]
    [SerializeField] float[] m_enemyBonus; // id 1 = base enemy; id 2 = shielded enemy; id 3 = bomberman
    [SerializeField] float m_healthRegenMalus = 15.0f;
    [SerializeField] float m_bonusPerSecond = 15.0f;

    [Header("UI References")]
    [SerializeField] Text m_scoreText = null;
    [SerializeField] Text m_finalScoreText = null;

    private float m_score;
    private float m_time;
    private bool m_isGameActive = false;
	
	// Update is called once per frame
	void Update ()
    {
		if (m_isGameActive)
        {
            m_time += Time.deltaTime;
            m_scoreText.text = m_score.ToString();
        }
	}

    /// <summary>
    /// The game has started condition
    /// </summary>
    /// <param name="i_isActive"></param>
    public void SetGameAsActive(bool i_isActive)
    {
        m_isGameActive = i_isActive;
    }

    /// <summary>
    /// Increse the total score by the amount defined in the inspictor based on the enemyType id
    /// </summary>
    /// <param name="enemyType"></param>
    public void ApplyEnemyBonus(int i_enemyType)
    {
        m_score += m_enemyBonus[i_enemyType];
    }

    /// <summary>
    /// Subtract from the total score everytime the player use a scene's light to regain some health back
    /// </summary>
    public void ApplyHealthRegenerationMalus()
    {
        m_score -= m_healthRegenMalus;
    }

    /// <summary>
    /// Calculate the final score adding the time bonus
    /// </summary>
    public void CalculateFinalScore()
    {
        //m_score += m_time * m_bonusPerSecond;
        m_finalScoreText.text = Mathf.RoundToInt(m_score).ToString();
    }
}
