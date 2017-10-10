using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Gameplay values")]
    public int m_enemyID = 0;

    //private float yOffset = 0.8f;

    public void Initialize(Vector3 position, Transform levelTransform)
    {
        float yOffset = this.GetComponent<BoxCollider>().bounds.size.y/2;
        name = "Enemy " + position.x + " " + position.z;
        transform.parent = levelTransform;
        transform.localPosition = new Vector3(position.x, position.y+yOffset, position.z);
    }
}
