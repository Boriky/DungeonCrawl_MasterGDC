using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Room m_roomPrefab = null;
    public GameObject m_playerPrefab = null;

    private Room m_roomInstance = null;
    private GameObject m_playerInstance = null;

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
    }

    // Update is called once per frame
    void Update()
    {
        // Activate player when the level has been generated
        if (!m_playerInstance.activeSelf && m_roomInstance.getIsGenerationOver())
        {
            m_playerInstance.SetActive(true);
        }

        // the player has been killed
            // destroy the player
            // game ends
    }
}
