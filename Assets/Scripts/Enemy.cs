﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    
    //private float yOffset = 0.8f;

    public void Initialize(float roomY, IntVector2 position)
    {
        float yOffset = this.GetComponent<BoxCollider>().bounds.size.y/2;
        name = "Enemy " + position.x + " " + position.z;
        //transform.parent = roomTransform;
        transform.localPosition = new Vector3(position.x, roomY+yOffset, position.z);
    }
}
