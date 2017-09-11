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

    [Header("UI references")]
    [SerializeField] Slider m_playerHealthBar = null;
    public Button m_abilityButton1 = null;
    public Button m_abilityButton2 = null;
    public Button m_abilityButton3 = null;
    public Button m_abilityButton4 = null;

    private Room m_roomInstance = null;
    private GameObject m_playerInstance = null;
    private Enemy[] m_enemies = null;
    private bool m_enemiesInitialized = false;

    private AIManager m_aiManager = null;

    private void Awake()
    {
        m_roomInstance = Instantiate(m_roomPrefab) as Room;
        m_roomInstance.Generate(new IntVector2(-1, -1), Directions.Direction.North);
        
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
