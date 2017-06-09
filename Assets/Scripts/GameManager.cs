using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private Room roomInstance;
    public Room roomPrefab;

	// Use this for initialization
	void Start () {
        BeginGame();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void BeginGame()
    {   

        roomInstance = Instantiate(roomPrefab) as Room;

        //TEMP: get a reference to the door of the previous room and pass the coordinates of the adjacent tile
        //TEMP: random initial direction
       StartCoroutine(roomInstance.Generate(new IntVector2(-1,-1), Directions.Direction.North));
    }
}
