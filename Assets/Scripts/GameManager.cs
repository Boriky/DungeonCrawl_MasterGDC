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
    private Enemy[] m_enemies;
    private bool enemiesInitialized = false;

    // Use this for initialization
    void Start()
    {
        m_roomInstance = Instantiate(m_roomPrefab) as Room;
        // TEMP: get a reference to the door of the previous room and pass the coordinates of the adjacent tile
        // TEMP: random initial direction
        StartCoroutine(m_roomInstance.Generate(new IntVector2(-1, -1), Directions.Direction.North));

        m_playerInstance = Instantiate(m_playerPrefab);
        m_playerInstance.transform.parent = m_roomInstance.transform;
        m_playerInstance.SetActive(false);
        m_enemies = new Enemy[ENEMY_NUMBER];
    }

    Enemy EnemySpawn()
    {
        Enemy enemy = Instantiate(m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Length)]);
        IntVector2 position = m_roomInstance.getRandomRoomPosition();
        float roomY = m_roomInstance.getHeight();
        enemy.Initialize(roomY, position);

        return enemy;
    }

    // Update is called once per frame
    void Update()
    {
        // Activate player when the level has been generated
        if (!m_playerInstance.activeSelf && m_roomInstance.getIsGenerationOver())
        {
            m_playerInstance.SetActive(true);
        }

        if (!enemiesInitialized && m_roomInstance.getIsGenerationOver())
        {
            for (int enemyIndex = 0; enemyIndex < m_enemies.Length - 1; ++enemyIndex)
            {
                Enemy enemy = EnemySpawn();
                m_enemies[enemyIndex] = enemy;
            }
            enemiesInitialized = true;
        }

        // the player has been killed
        // destroy the player
        // game ends
    }
}
