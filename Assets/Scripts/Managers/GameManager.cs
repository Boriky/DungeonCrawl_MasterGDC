using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Room settings")]
    [SerializeField] Room m_roomPrefab = null;

    [Header("Player settings")]
    [SerializeField] GameObject m_playerPrefab = null;

    [Header("Enemies settings")]
    [SerializeField] int ENEMY_NUMBER = 5;
    [SerializeField] GameObject[] m_enemyPrefabs = null;
    [SerializeField] int m_increseDifficulty = 2;

    [Header("Levels settings")]
    [SerializeField] GameObject[] m_levelPrefabs = null;
    [SerializeField] GameObject m_groundLevel = null;
    [SerializeField] Camera m_mainCamera = null;
    [SerializeField] GameObject m_scenery = null;
    [SerializeField] Light m_globalDirectLight = null;
    [SerializeField] Light m_playerLight = null;
    [SerializeField] Light m_divineLight = null;
    [SerializeField] MeshCollider m_coneCollider = null;
    [SerializeField] MeshRenderer m_coneRenderer = null;
    [SerializeField] BoxCollider m_levelTrigger = null;

    [Header("UI references")]
    [SerializeField] GameObject m_HUDPanel = null;
    [SerializeField] GameObject m_gameOverPanel = null;
    [SerializeField] GameObject m_gameLoadingPanel = null;
    [SerializeField] Slider m_playerHealthBar = null;
    [SerializeField] GameObject m_loadingTextRestart = null;
    [SerializeField] GameObject m_restartButton = null;
    [SerializeField] GameObject m_restartButtonBackground = null;
    public Button m_abilityButton1 = null;
    public Button m_abilityButton2 = null;
    public Button m_abilityButton3 = null;
    public Button m_abilityButton4 = null;

    [Header("Utility values")]
    public bool m_riseDirectLight = false;
    public float m_defaultPlayerLightSpotAngle = 0;
    public bool m_levelCompleted = false;     // TODO set to private

    private Room m_roomInstance = null;
    private GameObject m_playerInstance = null;
    private Enemy[] m_enemies = null;
    private GameObject[] m_levelInstances = null;
    public ScoreSystem m_scoreSystem = null;
    private AIManager m_aiManager = null;

    private bool m_enemiesInitialized = false;
    private int m_numberOfActiveEnemies = 0;
    private int m_currentLevelIndex = 0;

    private void Awake()
    {
        m_mainCamera = Camera.main;
        CreateProceduralRoom();
        SpawnLevels();
        SpawnCharacters();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // Use this for initialization
    private void Start()
    {
        m_aiManager = new AIManager();
        m_scoreSystem = GetComponent<ScoreSystem>();
        InitializeSkillBarAbilities();

        m_scoreSystem.SetGameAsActive(true);

        m_defaultPlayerLightSpotAngle = m_playerLight.spotAngle;
    }

    private void Update()
    {
        m_aiManager.MoveEnemies(m_enemies, m_playerInstance.transform.position);
        bool playerIsKinematic = m_playerInstance.GetComponent<Rigidbody>().isKinematic;

        // If there are no ememies left, start increasing the level global light
        if (m_numberOfActiveEnemies == 0)
        {
            m_riseDirectLight = true;
            // enter only one time in this condition
            m_numberOfActiveEnemies = 1;
        }

        if (m_riseDirectLight)
        {
            RiseGlobalDirectLightIntensity();
        }

        if (playerIsKinematic && m_playerInstance.activeInHierarchy)
        {
            m_gameLoadingPanel.SetActive(true);
            MaximizeGlobalIlluminationAndSetLevelCompleted();
        }

        if (m_levelCompleted)
        {
            StartLevelCompletedProcedure();
            m_gameLoadingPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Procedurally generate a new level
    /// </summary>
    private void CreateProceduralRoom()
    {
        m_roomInstance = Instantiate(m_roomPrefab) as Room;
        m_roomInstance.Generate(new IntVector2(-1, -1), Directions.Direction.North);
    }

    /// <summary>
    /// Generate a certain number of levels determined by the value "numberOfLevels" in a vertical tower with an offset of 22 units on the y axis
    /// </summary>
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

    /// <summary>
    /// Set the room position with an offset of 22 units on the y axis
    /// </summary>
    private void SetRoomPosition()
    {
        m_roomInstance.transform.parent = m_levelInstances[m_currentLevelIndex].transform;
        m_roomInstance.transform.position = new Vector3(m_roomInstance.transform.position.x, m_groundLevel.transform.position.y + (22.0f * /*m_currentLevelIndex + 1*/ 4), m_roomInstance.transform.position.z);
    }

    /// <summary>
    /// Spawn the player and the enemies based on the value ENEMY_NUMBER
    /// </summary>
    private void SpawnCharacters()
    {
        PlayerSpawn();
        EnemiesSpawn();
    }

    /// <summary>
    /// Instantiate the player and register it to its death event
    /// </summary>
    private void PlayerSpawn()
    {
        Vector3 playerPosition = new Vector3(m_roomInstance.transform.position.x, m_roomInstance.transform.position.y + 3, m_roomInstance.transform.position.z);
        m_playerInstance = Instantiate(m_playerPrefab, playerPosition, Quaternion.identity);
        m_playerInstance.transform.parent = m_roomInstance.transform;
        m_playerInstance.GetComponent<PlayerHealth>().m_onDeathEvent += OnPlayerDeath;
    }

    /// <summary>
    /// Instantiate all the enemies at random positions and register them to their death event
    /// </summary>
    private void EnemiesSpawn()
    {
        m_enemies = new Enemy[ENEMY_NUMBER];
        m_numberOfActiveEnemies = ENEMY_NUMBER;
        for (int enemyIndex = 0; enemyIndex < m_numberOfActiveEnemies; ++enemyIndex)
        {
            Enemy enemy = Instantiate(m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Length)]).GetComponent<Enemy>();
            Vector3 position = m_roomInstance.getSpawningPosition();
            enemy.Initialize(position, m_roomInstance.transform);
            enemy.GetComponent<EnemyHealth>().m_onDeathEvent += OnEnemyDeath;
            m_enemies[enemyIndex] = enemy;
        }
    }

    /// <summary>
    /// Register the player's abilities to the respective skillbar's buttons
    /// </summary>
    private void InitializeSkillBarAbilities()
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

    /// <summary>
    /// Move the camera 22 units down, enable the divine light and its trigger, procedurally generate new level, set the new level position, spawn the enemies and call the destruction of the current level
    /// </summary>
    private void CreateNewLevel()
    {
        m_mainCamera.transform.position = new Vector3(m_mainCamera.transform.position.x, m_mainCamera.transform.position.y /*- 22.0f*/, m_mainCamera.transform.position.z);
        m_divineLight.enabled = false;
        m_divineLight.transform.position = new Vector3(m_divineLight.transform.position.x, m_divineLight.transform.position.y /*- 22.0f*/, m_divineLight.transform.position.z);
        m_playerInstance.transform.parent = null;
        Destroy(m_roomInstance.gameObject);
        CreateProceduralRoom();
        // Destroy(m_levelInstances[m_currentLevelIndex]);
        // m_currentLevelIndex--;
        SetRoomPosition();
        ENEMY_NUMBER += m_increseDifficulty;
        EnemiesSpawn();
        m_levelCompleted = false;
    }

    /// <summary>
    /// Destroy the player and show the gameover results
    /// </summary>
    /// <param name="i_listener"></param>
    private void OnPlayerDeath(PlayerHealth i_listener)
    {
        if (i_listener != null)
        {
            //Destroy(i_listener.gameObject);
            i_listener.gameObject.SetActive(false);
            m_HUDPanel.SetActive(false);
            //m_HUDPanel.SetActive(false);
            m_gameOverPanel.SetActive(true);
            m_scoreSystem.CalculateFinalScore();
            // Destroy GameObject
                // Particles etc
            // GameOver
            // Ask for Game Restart
        }
    }

    /// <summary>
    /// Destroy the enemy, apply a score bonus based on the enemy type and update the number of currently active enemies
    /// </summary>
    /// <param name="i_listener"></param>
    private void OnEnemyDeath(EnemyHealth i_listener, int i_enemyID)
    {
        /*Enemy enemyInstance = i_Listener.transform.parent.GetComponent<Enemy>();
        int enemyId = enemyInstance.m_enemyID;*/

        m_scoreSystem.ApplyEnemyBonus(i_enemyID);

        if (i_listener != null)
        {
            Destroy(i_listener.gameObject);
            m_numberOfActiveEnemies--;
        }
    }

    /// <summary>
    /// Slowly rise the global light intensity
    /// </summary>
    private void RiseGlobalDirectLightIntensity()
    {
        m_globalDirectLight.enabled = true;
        m_globalDirectLight.intensity += 0.008f;

        m_divineLight.enabled = true;
        m_divineLight.intensity += 0.01f;

        if (m_globalDirectLight.intensity > 0.7f)
        {
            m_coneCollider.enabled = true;
            m_coneRenderer.enabled = true;
            m_levelTrigger.enabled = true;
            m_riseDirectLight = false;
        }
    }

    /// <summary>
    /// Procedurally icrease the brightness of the global direct light to the highest value and set the level as completed
    /// </summary>
    private void MaximizeGlobalIlluminationAndSetLevelCompleted()
    {
        m_globalDirectLight.intensity += 0.35f;

        if (m_globalDirectLight.intensity > 15)
        {
            m_levelCompleted = true;
        }
    }

    /// <summary>
    /// Create new level, restore the global light settings to darkness, turn off player as kinematic, give the player back some of it's light/health, disable the divine light and its trigger
    /// </summary>
    private void StartLevelCompletedProcedure()
    {
        /*if (m_currentLevelIndex != 0)
        {*/
            // generate new level
            CreateNewLevel();
            // set direct light to 0
            m_globalDirectLight.intensity = 0.0f;
            // set spotlight to whatever it was
            m_divineLight.intensity = 0.0f;

            m_playerInstance.GetComponent<Rigidbody>().isKinematic = false;
            m_playerInstance.GetComponent<OnLightCrossed>().hasEntered = false;

            // give player some of its light back if he has been damanged
            if (m_playerLight.spotAngle < m_defaultPlayerLightSpotAngle)
            {
                m_playerLight.spotAngle += 10.0f;
                if (m_playerLight.spotAngle > m_defaultPlayerLightSpotAngle)
                {
                    m_playerLight.spotAngle = m_defaultPlayerLightSpotAngle;
                }
            }

            m_coneCollider.enabled = false;
            m_coneRenderer.enabled = false;
            m_levelTrigger.enabled = false;

            m_levelCompleted = false;
        /*}
        else
        {
            // game completed
        }*/
    }

    /// <summary>
    /// Returns the player's healthbar
    /// </summary>
    public Slider GetPlayerHealthBar()
    {
        return m_playerHealthBar;
    }

    /// <summary>
    /// Force restart of the current scene
    /// </summary>
    public void RestartGame()
    {
        m_restartButtonBackground.SetActive(false);
        m_restartButton.SetActive(false);
        m_loadingTextRestart.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// For playtesting when enemies get stuck...
    /// </summary>
    public void ForceLevelCompleted()
    {
        m_levelCompleted = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}