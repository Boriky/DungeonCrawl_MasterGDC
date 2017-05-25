using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public float m_moveTime = 1.5f;
    public float m_movementSpeed = 10f;
    public GameObject[] m_colliders = new GameObject[4];

    private float m_timePassed = 0f;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        m_timePassed += Time.deltaTime;
        if (m_timePassed >= m_moveTime) {
            int condition = Random.Range(0, 3);
            if (condition == 0 && m_colliders[0].GetComponent<EnemyDetectCollision>().CanMoveHere())
                transform.position += transform.forward * Time.deltaTime * m_movementSpeed;
            else if (condition == 1 && m_colliders[1].GetComponent<EnemyDetectCollision>().CanMoveHere())
                transform.position -= transform.forward * Time.deltaTime * m_movementSpeed;
            else if (condition == 2 && m_colliders[2].GetComponent<EnemyDetectCollision>().CanMoveHere())
                transform.position += transform.right * Time.deltaTime * m_movementSpeed;
            else if (condition == 3 && m_colliders[3].GetComponent<EnemyDetectCollision>().CanMoveHere())
                transform.position -= transform.right * Time.deltaTime * m_movementSpeed;

            for (int index = 0; index < 3; ++index) {
                m_colliders[index].GetComponent<EnemyDetectCollision>().resetMoveHere();
            }

            m_timePassed = 0f;
        }
    }
}
