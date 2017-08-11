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
    [SerializeField] Enemy[] m_enemyPrefabs = null;

    [Header("UI references")]
    [SerializeField] Slider m_playerHealthBar = null;
    public Button[] m_playerAbilitiesButtons = new Button[4];

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

    private void Start()
    {
        InitializeSkillBarAbilities();
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

    void InitializeSkillBarAbilities()
    {
        Jump jumpSkill = m_playerInstance.GetComponent<Jump>();
        RollOver rollSkill = m_playerInstance.GetComponent<RollOver>();
        ShootForward shootForwardSkill = m_playerInstance.GetComponent<ShootForward>();
        ShootAround shootAroundSkill = m_playerInstance.GetComponent<ShootAround>();

        m_playerAbilitiesButtons[0].onClick.AddListener(jumpSkill.PerformJump);
        m_playerAbilitiesButtons[1].onClick.AddListener(rollSkill.PerformRollOver);
        m_playerAbilitiesButtons[2].onClick.AddListener(shootForwardSkill.FireProjectile);
        m_playerAbilitiesButtons[3].onClick.AddListener(shootAroundSkill.FireProjectiles);
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

    public Slider GetPlayerHealthBar()
    {
        return m_playerHealthBar;
    }
}
