using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base enemy class. Stores the enemy ID expressed by a simple int
public class Enemy : MonoBehaviour
{
    [Header("Gameplay values")]
    public int m_enemyID = 0;

    //private float yOffset = 0.8f;

    /// <summary>
    /// Setup the enemy spawn position
    /// </summary>
    public void Initialize(Vector3 position, Transform levelTransform)
    {
        float yOffset = this.GetComponent<BoxCollider>().bounds.size.y/2;
        name = "Enemy " + position.x + " " + position.z;
        transform.parent = levelTransform;
        transform.localPosition = new Vector3(position.x, position.y+yOffset, position.z);
    }
}
