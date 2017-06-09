using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public float generationStepDelay;

    public IntVector2 size;

    public RoomComponent[] roomComponentPrefabs;
    private RoomComponent[,] roomComponents;

    private bool isGenerationOver;
    //TEMP
    private IntVector2 playerStartPosition;

    public IEnumerator Generate(IntVector2 doorPreviousCoordinates, Directions.Direction doorPreviousDirection)
    {
        isGenerationOver = false;
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        IntVector2 startPositionForGeneration;
        if (!IsInvalidRoomPosition(doorPreviousCoordinates))
        {
            //GENERATE STARTING DOOR IN ROOM AND START GENERATION OF ROOM FROM THIS TILE
            playerStartPosition = ConvertToNewRoomCoordinates(doorPreviousCoordinates, doorPreviousDirection);
        } else
        {
            playerStartPosition = new IntVector2(0, Random.Range(0, size.z));
        }

        startPositionForGeneration = playerStartPosition;
        roomComponents = new RoomComponent[size.x, size.z];

        List<RoomComponent> activeComponents = new List<RoomComponent>();

        DoFirstGenerationStep(startPositionForGeneration, doorPreviousDirection, activeComponents);

        while (activeComponents.Count > 0)
        {
            yield return false;
            DoNextGenerationStep(activeComponents);
        }
        isGenerationOver = true;
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
                    neighbour = Instantiate(roomComponentPrefabs[Random.Range(0, roomComponentPrefabs.Length)]) as RoomComponent;
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
        int randomPrefabIndex = Random.Range(0, roomComponentPrefabs.Length);
        //make sure we instantiate a safe component
        while (!roomComponentPrefabs[randomPrefabIndex].safe)
        {
            randomPrefabIndex = Random.Range(0, roomComponentPrefabs.Length);
        }
        RoomComponent component = Instantiate(roomComponentPrefabs[randomPrefabIndex]) as RoomComponent;
        return component;
    }

    private void CreateNeighbour()
    {

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
}
