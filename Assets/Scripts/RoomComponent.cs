using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomComponent : MonoBehaviour {

    public bool safe;

    private int initializedNeighbourCount = 0;
    private int nonNullNeighbours;
    private int safeNeighbours;
    private IntVector2 coordinates;

    private RoomComponent[] neighbours = new RoomComponent[Directions.Count];
    private bool[] neighbourInitialized = new bool[Directions.Count];

    public void Initialize(IntVector2 coordinates, Transform roomTransform)
    {
        this.coordinates = coordinates;
        name = "Room Component " + coordinates.x + ", " + coordinates.z;
        transform.parent = roomTransform;
        transform.localPosition = new Vector3(coordinates.x * 9.0f, 0f, coordinates.z * 9.0f + 4.5f);
    }

    public IntVector2 GetCoordinates()
    {
        return this.coordinates;
    }

    public bool IsNeighbourAlreadyInitialized(Directions.Direction direction)
    {
        return neighbourInitialized[(int)direction];
    }

    public bool IsFullyInitialized()
    {
        return initializedNeighbourCount == neighbours.Length;
    }

    public void AddNeighbour(RoomComponent neighbour, Directions.Direction direction)
    {
        neighbours[(int)direction] = neighbour;
        neighbourInitialized[(int)direction] = true;
        initializedNeighbourCount++;

        if (neighbour != null)
        {
            nonNullNeighbours++;

            if (neighbour.safe)
            {
                safeNeighbours++;
            }
        }
    }


    public bool NeedsSafeNeighbour()
    {
        if (safeNeighbours <= Mathf.Ceil(nonNullNeighbours / 2f) && initializedNeighbourCount > 0)
        {
            return true;
        }
        return false;
    }

    public Directions.Direction RandomUninitializedDirection()
    {
        int skips = Random.Range(0, Directions.Count - initializedNeighbourCount);
        for (int i = 0; i < Directions.Count; ++i)
        {
            if (!neighbourInitialized[i])
            {
                if (skips == 0)
                {
                    return (Directions.Direction)i;
                }
                skips -= 1;
            }
        }
        throw new System.InvalidOperationException("RoomComponent has no uninitialized directions left");
    }
}
