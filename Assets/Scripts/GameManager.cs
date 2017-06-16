using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int ENEMY_NUMBER = 5;

    public Room m_roomPrefab = null;
    public GameObject m_playerPrefab = null;
    public Enemy[] m_enemyPrefabs = null;
    
    private Room m_roomInstance = null;
    private GameObject m_playerInstance = null;
    private Enemy[] m_enemies = null;
    private bool m_enemiesInitialized = false;

    private AIManager m_aiManager = null;
    // Use this for initialization
    void Awake()
    {
        
        m_roomInstance = Instantiate(m_roomPrefab) as Room;
        m_roomInstance.Generate(new IntVector2(-1, -1), Directions.Direction.North);

        SpawnCharacters();

    }

    private void SpawnCharacters()
    {
        PlayerSpawn();

        m_enemies = new Enemy[ENEMY_NUMBER];
        EnemiesSpawn();

    }

    void PlayerSpawn()
    {
        m_playerInstance = Instantiate(m_playerPrefab);
        m_playerInstance.transform.parent = m_roomInstance.transform;
        m_playerInstance.GetComponent<PlayerHealth>().m_onDeathEvent += onPlayerDeath;
    }

    void EnemiesSpawn()
    {
        for (int enemyIndex = 0; enemyIndex < m_enemies.Length - 1; ++enemyIndex)
        {
            Enemy enemy = Instantiate(m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Length)]);
            Vector3 position = m_roomInstance.getSpawningPosition();
            enemy.Initialize(position, m_roomInstance.transform);
            enemy.GetComponent<EnemyHealth>().m_onDeathEvent += OnEnemyDeath;
            m_enemies[enemyIndex] = enemy;
        }
    }

    private void onPlayerDeath(PlayerHealth i_Listener)
    {
        if (i_Listener != null)
        {
            // Destroy GameObject
                // Particles etc
            // GameOver
            // Ask for Game Restart
        }
    }

    private void OnEnemyDeath(EnemyHealth i_Listener)
    {
        if (i_Listener != null)
        {
            Destroy(i_Listener.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
