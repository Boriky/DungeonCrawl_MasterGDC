using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectCollision : MonoBehaviour {

    private bool m_moveHere = true;

    public bool CanMoveHere() { return m_moveHere; }

    public void resetMoveHere() { m_moveHere = true; }

    void OnCollisionEnter(Collision col) {
        if (gameObject.tag == "Enemy" && col.gameObject.tag == "Player") {
            Destroy(gameObject.transform.parent.gameObject);
            Destroy(gameObject);
        }

        if (col.gameObject.tag == "Wall") {
            m_moveHere = false;
        }
    }
}
