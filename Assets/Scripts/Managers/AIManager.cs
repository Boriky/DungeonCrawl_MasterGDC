using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager {

    public void MoveEnemies(Enemy[] enemies, Vector3 position)
    {
        foreach (Enemy currentEnemy in enemies) {
            currentEnemy.GetComponent<NavMeshAgent>().destination = position;
        }
    }
}
