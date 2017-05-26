using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public float m_moveTime = 1.5f;
    public float m_movementSpeed = 10f;

    private float m_timePassed = 0f;

    // Update is called once per frame
    void Update() {
        m_timePassed += Time.deltaTime;
        if (m_timePassed >= m_moveTime) {
            int condition = Random.Range(0, 3);
            if (condition == 0)
                transform.position += transform.forward * Time.deltaTime * m_movementSpeed;
            else if (condition == 1)
                transform.position -= transform.forward * Time.deltaTime * m_movementSpeed;
            else if (condition == 2)
                transform.position += transform.right * Time.deltaTime * m_movementSpeed;
            else if (condition == 3)
                transform.position -= transform.right * Time.deltaTime * m_movementSpeed;

            m_timePassed = 0f;
        }
    }
}
