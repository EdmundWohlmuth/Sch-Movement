using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ArenaManager : MonoBehaviour
{
    int width = 200;
    public GameObject door;
    public List<Door> doors;

    public DoorDirection lastOpenDoor;

    public GameObject arenaToSpawn;

    private bool hasBeenSpawned = false;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        if (!hasBeenSpawned) Spawned();
    }

    // Start is called before the first frame update
    void Spawned(DoorDirection dir = DoorDirection.None)
    {
        GameManager.gameManager.spawnedPositions[Vector3Int.RoundToInt(gameObject.transform.position)] = this;
        hasBeenSpawned = true;
        GameManager.gameManager.arenaManager = this;
        // Set doors to only new doors
        doors.Clear();

        Debug.Log($"Arena spawned at {Vector3Int.RoundToInt(transform.position)}");
        foreach (Transform door in door.transform)
            if (door.gameObject.TryGetComponent<Door>(out Door foundDoor))
                foundDoor.SetupDirection(this);

        GameManager.gameManager.arenaManager = gameObject.GetComponent<ArenaManager>();
        //GameManager.gameManager.enemySpawner = gameObject.GetComponent<EnemySpawner>();

        // Open doors
        if (dir != DoorDirection.None)
        {
            lastOpenDoor = dir;
            OpenLastDoor();
        }
    }

    public void OpenLastDoor(DoorDirection door = DoorDirection.None)
    {
        if (door != DoorDirection.None) lastOpenDoor = door;
        if (lastOpenDoor == 0) return;

        Debug.Log($"Arena {Vector3Int.RoundToInt(transform.position)} is opening door direction {lastOpenDoor}");
        for (int i = 0; i < doors.Count; i++)
            if (doors[i].direction == lastOpenDoor && doors[i].gameObject.activeInHierarchy)
                doors[i].gameObject.SetActive(false);
    }

    public Vector3Int GetNewSpawnPosition(DoorDirection chosenDirection)
    {
        switch (chosenDirection)
        {
            case DoorDirection.South:
                return Vector3Int.RoundToInt(transform.position + (width * -Vector3Int.forward));
            case DoorDirection.North:
                return Vector3Int.RoundToInt(transform.position + (width * Vector3Int.forward));
            case DoorDirection.West:
                return Vector3Int.RoundToInt(transform.position + (width * -Vector3Int.right));
            case DoorDirection.East:
                return Vector3Int.RoundToInt(transform.position + (width * Vector3Int.right));
        }
        return Vector3Int.zero;
    }

    public Vector3Int ArenaId => Vector3Int.RoundToInt(transform.position);

    public bool SpawnArena(DoorDirection chosenDirection)
    {
        Debug.Log($"My arena {ArenaId} is trying to spawn an arena on the direction {chosenDirection} which would result in Arena ID {GetNewSpawnPosition(chosenDirection)} Does it alreayd exist? {GameManager.gameManager.spawnedPositions.ContainsKey(GetNewSpawnPosition(chosenDirection))}");
        GameManager.gameManager.posToSpawn = GetNewSpawnPosition(chosenDirection);
        if (GameManager.gameManager.spawnedPositions.TryGetValue(GameManager.gameManager.posToSpawn, out ArenaManager otherArena))
        {
            Debug.Log($"I {ArenaId} Cannot Spawn arena in direction {chosenDirection} already have arena at {GameManager.gameManager.posToSpawn} open door in {OppositeDirection(chosenDirection)}");

            otherArena.OpenLastDoor(OppositeDirection(chosenDirection));

            OpenLastDoor(chosenDirection);
            return false;
        }

        OpenLastDoor(chosenDirection);

        NavMesh.RemoveAllNavMeshData();
        ArenaManager newArena = Instantiate(GameManager.gameManager.arenaToSpawn.GetRandom(), GameManager.gameManager.posToSpawn, Quaternion.Euler(0, UnityEngine.Random.Range(0, 4) * 90, 0)).GetComponent<ArenaManager>();
        newArena.transform.SetParent(GameObject.Find("ArenaHolder").transform);
        newArena.gameObject.SetActive(true);
        newArena.Spawned(OppositeDirection(chosenDirection));

        StartCoroutine(SpawnEnemies(newArena));

        return true;
    }

    public DoorDirection OppositeDirection(DoorDirection desiredDirection)
    {
        return desiredDirection switch
        {
            DoorDirection.South => (DoorDirection.North),
            DoorDirection.North => (DoorDirection.South),
            DoorDirection.West => (DoorDirection.East),
            DoorDirection.East => (DoorDirection.West),
            _ => DoorDirection.None
        };
    }

    public IEnumerator SpawnEnemies(ArenaManager newArena)
    {
        yield return new WaitForSeconds(.1f);
        newArena.GetComponent<EnemySpawner>().BeginSpawn();
        Door.SetDoorState(false);
    }
}
