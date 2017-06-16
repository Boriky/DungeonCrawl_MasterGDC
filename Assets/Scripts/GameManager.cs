using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private Room roomInstance;
    public Room roomPrefab;

    public GameObject playerPrefab;
    private GameObject playerInstance;


    public Enemy[] enemyPrefabs;
    public int ENEMY_NUMBER = 5;
    private Enemy[] enemies;
    private bool enemiesInitialized = false;

    // Use this for initialization
    void Start () {
        BeginGame();
	}
	
	// Update is called once per frame
	void Update () {
        if (!playerInstance.activeSelf && roomInstance.getIsGenerationOver())
        {
            playerInstance.SetActive(true);
        }

        if (!enemiesInitialized && roomInstance.getIsGenerationOver())
        {
            for (int enemyIndex = 0; enemyIndex < enemies.Length - 1; ++enemyIndex)
            {
                Enemy enemy = EnemySpawn();
                enemies[enemyIndex] = enemy;
            }
            enemiesInitialized = true;
        }

    }

    void BeginGame()
    {   

        roomInstance = Instantiate(roomPrefab) as Room;

        //TEMP: get a reference to the door of the previous room and pass the coordinates of the adjacent tile
        //TEMP: random initial direction
        StartCoroutine(roomInstance.Generate(new IntVector2(-1,-1), Directions.Direction.North));
        
        playerInstance = Instantiate(playerPrefab);
        playerInstance.SetActive(false);
        enemies = new Enemy[ENEMY_NUMBER];
    }

    Enemy EnemySpawn()
    {
            Enemy enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
            IntVector2 position = roomInstance.getRandomRoomPosition();
            float roomY = roomInstance.getHeight();
            enemy.Initialize(roomY, position);
        
        return enemy;
    }
}
