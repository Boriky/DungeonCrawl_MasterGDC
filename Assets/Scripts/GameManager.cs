﻿using System.Collections;
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

    // Use this for initialization
    void Start()
    {
        m_roomInstance = Instantiate(m_roomPrefab) as Room;
        // TEMP: get a reference to the door of the previous room and pass the coordinates of the adjacent tile
        // TEMP: random initial direction
        StartCoroutine(m_roomInstance.Generate(new IntVector2(-1, -1), Directions.Direction.North, SpawnCharacters));

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
    }

    void EnemiesSpawn()
    {
        for (int enemyIndex = 0; enemyIndex < m_enemies.Length - 1; ++enemyIndex)
        {
            Enemy enemy = Instantiate(m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Length)]);
            Vector3 position = m_roomInstance.getSpawningPosition();
            enemy.Initialize(position, m_roomInstance.transform);
            m_enemies[enemyIndex] = enemy;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
