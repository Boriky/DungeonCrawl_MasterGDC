using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public IntVector2 size;

    public RoomComponent[] roomComponentPrefabs;
    private RoomComponent[,] roomComponents;

    private bool isGenerationOver;
    //TEMP
    private IntVector2 playerStartPosition;

    public void Generate(IntVector2 doorPreviousCoordinates, Directions.Direction doorPreviousDirection)
    {

        IntVector2 startPositionForGeneration;
        if (!IsInvalidRoomPosition(doorPreviousCoordinates))
        {
            //GENERATE STARTING DOOR IN ROOM AND START GENERATION OF ROOM FROM THIS TILE
            playerStartPosition = ConvertToNewRoomCoordinates(doorPreviousCoordinates, doorPreviousDirection);
        } else
        {
            playerStartPosition = new IntVector2(0, UnityEngine.Random.Range(0, size.z));
        }

        startPositionForGeneration = playerStartPosition;
        roomComponents = new RoomComponent[size.x, size.z];

        List<RoomComponent> activeComponents = new List<RoomComponent>();

        DoFirstGenerationStep(startPositionForGeneration, doorPreviousDirection, activeComponents);

        while (activeComponents.Count > 0)
        {
            DoNextGenerationStep(activeComponents);
        }

     }

    private void DoFirstGenerationStep(IntVector2 startPositionForGeneration, Directions.Direction doorPreviousDirection, List<RoomComponent> activeComponents)
    {
        RoomComponent firstComponent = InstantiateSafeComponent();
        roomComponents[startPositionForGeneration.x, startPositionForGeneration.z] = firstComponent;
        firstComponent.Initialize(startPositionForGeneration, transform, size);

        activeComponents.Add(firstComponent);

    }

    //TODO: METTERE PORTE
    private void DoNextGenerationStep(List<RoomComponent> activeComponents)
    {
        
        int currentIndex = activeComponents.Count - 1;

        RoomComponent currentComponent = activeComponents[currentIndex];
        if (currentComponent.IsFullyInitialized())
        {
            activeComponents.RemoveAt(currentIndex);
            return;
        }

        Directions.Direction direction = currentComponent.RandomUninitializedDirection();

        IntVector2 newComponentCoordinates = currentComponent.GetCoordinates() + direction.ToIntVector2();
        
        if (!IsInvalidRoomPosition(newComponentCoordinates))
        {
            RoomComponent neighbour = roomComponents[newComponentCoordinates.x, newComponentCoordinates.z];

            if (neighbour == null)
            {
                if (currentComponent.NeedsSafeNeighbour())
                {
                    neighbour = InstantiateSafeComponent();
                }
                else
                {
                    neighbour = Instantiate(roomComponentPrefabs[UnityEngine.Random.Range(0, roomComponentPrefabs.Length)]) as RoomComponent;
                }
                neighbour.Initialize(newComponentCoordinates, transform, size);
                roomComponents[newComponentCoordinates.x, newComponentCoordinates.z] = neighbour;
                currentComponent.AddNeighbour(neighbour, direction);
                neighbour.AddNeighbour(currentComponent, direction.GetOpposite());
                activeComponents.Add(neighbour);
            }
            else
            {
                if (!currentComponent.IsNeighbourAlreadyInitialized(direction))
                {
                    currentComponent.AddNeighbour(neighbour, direction);
                    neighbour.AddNeighbour(currentComponent, direction.GetOpposite());
                }
            }
        }  else
        {
            currentComponent.AddNeighbour(null, direction);

        }

    }

    private RoomComponent InstantiateSafeComponent()
    {
        int randomPrefabIndex = UnityEngine.Random.Range(0, roomComponentPrefabs.Length);
        //make sure we instantiate a safe component
        while (!roomComponentPrefabs[randomPrefabIndex].safe)
        {
            randomPrefabIndex = UnityEngine.Random.Range(0, roomComponentPrefabs.Length);
        }
        RoomComponent component = Instantiate(roomComponentPrefabs[randomPrefabIndex]) as RoomComponent;
        return component;
    }

    private bool IsInvalidRoomPosition(IntVector2 position)
    {
        if (position.x < 0 || position.x >= size.x || position.z < 0 || position.z >= size.z)
        {
            return true;
        }
        return false;
    }

    private IntVector2 ConvertToNewRoomCoordinates(IntVector2 oldRoomCoordinates, Directions.Direction edgeDirection)
    {
        IntVector2 newRoomCoordinates;
        if (edgeDirection == Directions.Direction.North)
        {
            newRoomCoordinates = new IntVector2(oldRoomCoordinates.x, 0);
        } else if (edgeDirection == Directions.Direction.South)
        {
            newRoomCoordinates = new IntVector2(oldRoomCoordinates.x, size.z);
        } else if (edgeDirection == Directions.Direction.East)
        {
            newRoomCoordinates = new IntVector2(0, oldRoomCoordinates.z);
        } else if (edgeDirection == Directions.Direction.West)
        {
            newRoomCoordinates = new IntVector2(size.x, oldRoomCoordinates.z);
        } else
        {
            newRoomCoordinates = new IntVector2(-1,-1);
        }
        return newRoomCoordinates;
    }

    public bool getIsGenerationOver()
    {
        return isGenerationOver;
    }

    public Vector3 getSpawningPosition()
    {
        RoomComponent component = roomComponents[UnityEngine.Random.Range(0, size.x), UnityEngine.Random.Range(0, size.z)];
        IntVector2 positionInComponent = component.getRandomComponentPosition();

        Vector3 componentCoordinates = component.GetPosition();
        float xCoordinate = componentCoordinates.x;
        float yCoordinate = getHeight(component);
        float zCoordinate = componentCoordinates.z;

        return new Vector3(xCoordinate + positionInComponent.x, yCoordinate, zCoordinate + positionInComponent.z);
    }

    public float getHeight(RoomComponent component)
    {
        return component.GetComponentInChildren<MeshRenderer>().bounds.size.y;
    }
}
