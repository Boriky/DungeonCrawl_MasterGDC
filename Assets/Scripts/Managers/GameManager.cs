using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Room settings")]
    [SerializeField] Room m_roomPrefab = null;

    [Header("Player settings")]
    [SerializeField] GameObject m_playerPrefab = null;

    [Header("Enemies settings")]
    [SerializeField] int ENEMY_NUMBER = 5;
    [SerializeField] GameObject[] m_enemyPrefabs = null;

    [Header("Levels settings")]
    [SerializeField] GameObject[] m_levelPrefabs = null;
    [SerializeField] GameObject m_groundLevel = null;
    [SerializeField] Camera m_mainCamera = null;
    [SerializeField] GameObject m_scenery = null;

    [Header("UI references")]
    [SerializeField] Slider m_playerHealthBar = null;
    public Button m_abilityButton1 = null;
    public Button m_abilityButton2 = null;
    public Button m_abilityButton3 = null;
    public Button m_abilityButton4 = null;

    private Room m_roomInstance = null;
    private GameObject m_playerInstance = null;
    private Enemy[] m_enemies = null;
    private GameObject[] m_levelInstances = null;
    private bool m_enemiesInitialized = false;
    public bool m_levelCompleted = false;     // TODO set to private
    private int m_numberOfActiveEnemies = 0;
    private int m_currentLevelIndex = 0;

    private AIManager m_aiManager = null;

    private void Awake()
    {
        m_mainCamera = Camera.main;
        CreateProceduralRoom();
        SpawnLevels();
        SpawnCharacters();
    }

    // Use this for initialization
    private void Start()
    {
        m_aiManager = new AIManager();
        InitializeSkillBarAbilities();
        
    }

    private void Update()
    {
        m_aiManager.MoveEnemies(m_enemies, m_playerInstance.transform.position);
        if (m_levelCompleted)
        {
            if (m_currentLevelIndex!=0)
            {
                CreateNewLevel();
            }
            else
            {
                // game completed
            }
        }

        if (m_numberOfActiveEnemies == 0)
        {
            // Open the trapdoor for the next level
            // if the player cross the trapdoor, set the player as kinematic, m_levelCompleted = true and generate new level!
            m_levelCompleted = true;
        }
    }

    private void CreateProceduralRoom()
    {
        m_roomInstance = Instantiate(m_roomPrefab) as Room;
        m_roomInstance.Generate(new IntVector2(-1, -1), Directions.Direction.North);
    }

    private void SpawnLevels()
    {
        int numberOfLevels = m_levelPrefabs.Length + 1;
        m_levelInstances = new GameObject[numberOfLevels];

        m_levelInstances[m_currentLevelIndex] = m_groundLevel.gameObject;
        m_currentLevelIndex++;

        while (m_currentLevelIndex < numberOfLevels)
        {
            float xPosition = m_groundLevel.transform.position.x;
            float yPosition = m_groundLevel.transform.position.y + (22.0f * m_currentLevelIndex + 1);
            float zPosition = m_groundLevel.transform.position.z;
            Vector3 levelPosition = new Vector3(xPosition, yPosition, zPosition);
            m_levelInstances[m_currentLevelIndex] = Instantiate(m_levelPrefabs[m_currentLevelIndex - 1], levelPosition, Quaternion.identity);
            m_levelInstances[m_currentLevelIndex].transform.parent = m_scenery.transform;

            if (m_currentLevelIndex == numberOfLevels - 1)
            {
                SetRoomPosition();
            }

            m_currentLevelIndex++;
        }

        m_currentLevelIndex--;
    }

    private void SetRoomPosition()
    {
        m_roomInstance.transform.parent = m_levelInstances[m_currentLevelIndex].transform;
        m_roomInstance.transform.position = new Vector3(m_roomInstance.transform.position.x, m_groundLevel.transform.position.y + (22.0f * m_currentLevelIndex + 1), m_roomInstance.transform.position.z);
    }

    private void SpawnCharacters()
    {
        PlayerSpawn();

        m_enemies = new Enemy[ENEMY_NUMBER];
        EnemiesSpawn();
    }

    void PlayerSpawn()
    {
        Vector3 playerPosition = new Vector3(m_roomInstance.transform.position.x, m_roomInstance.transform.position.y + 3, m_roomInstance.transform.position.z);
        m_playerInstance = Instantiate(m_playerPrefab, playerPosition, Quaternion.identity);
        m_playerInstance.transform.parent = m_roomInstance.transform;
        m_playerInstance.GetComponent<PlayerHealth>().m_onDeathEvent += onPlayerDeath;
    }

    void EnemiesSpawn()
    {
        m_numberOfActiveEnemies = ENEMY_NUMBER;
        for (int enemyIndex = 0; enemyIndex < m_enemies.Length; ++enemyIndex)
        {
            Enemy enemy = Instantiate(m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Length)]).GetComponent<Enemy>();
            Vector3 position = m_roomInstance.getSpawningPosition();
            enemy.Initialize(position, m_roomInstance.transform);
            enemy.GetComponent<EnemyHealth>().m_onDeathEvent += OnEnemyDeath;
            m_enemies[enemyIndex] = enemy;
        }
    }

    void InitializeSkillBarAbilities()
    {
        Jump jumpSkill = m_playerInstance.GetComponent<Jump>();
        RollOver rollSkill = m_playerInstance.GetComponent<RollOver>();
        ShootForward shootForwardSkill = m_playerInstance.GetComponent<ShootForward>();
        ShootAround shootAroundSkill = m_playerInstance.GetComponent<ShootAround>();

        m_abilityButton1.onClick.AddListener(jumpSkill.PerformJump);
        m_abilityButton2.onClick.AddListener(rollSkill.PerformRollOver);
        m_abilityButton3.onClick.AddListener(shootForwardSkill.FireProjectile);
        m_abilityButton4.onClick.AddListener(shootAroundSkill.FireProjectiles);
    }

    void CreateNewLevel()
    {
        m_mainCamera.transform.position = new Vector3(m_mainCamera.transform.position.x, m_mainCamera.transform.position.y - 22.0f, m_mainCamera.transform.position.z);
        m_playerInstance.transform.parent = null;
        CreateProceduralRoom();
        Destroy(m_levelInstances[m_currentLevelIndex]);
        m_currentLevelIndex--;
        SetRoomPosition();
        EnemiesSpawn();
        m_levelCompleted = false;
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
            m_numberOfActiveEnemies--;
        }
    }

    public Slider GetPlayerHealthBar()
    {
        return m_playerHealthBar;
    }
}
