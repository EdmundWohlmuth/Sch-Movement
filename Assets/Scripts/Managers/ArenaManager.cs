using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager arenaManager;

    int width = 200;
    int arenasCompleted;

    public GameObject arenaToSpawn;
    public GameObject doors;
    public Vector3 posToSpawn;
    public List<Vector3> spawnedPositions = new List<Vector3>();

    private void Awake()
    {
        if (arenaManager == null)
        {
            arenaManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (arenaManager != this && arenaManager != null)
        {
            Destroy(this.gameObject);
        }

        spawnedPositions.Add(Vector3.zero);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOpenable()
    {

    }

    public void SpawnArena(int chosenDirection)
    {
        switch (chosenDirection)
        {
            case 1:
                Debug.Log("north");
                posToSpawn = new Vector3(posToSpawn.x + width, 0, posToSpawn.z);
                break;

            case 2:
                Debug.Log("south");
                posToSpawn = new Vector3(posToSpawn.x - width, 0, posToSpawn.z);

                break;

            case 3:
                Debug.Log("east");
                posToSpawn = new Vector3(posToSpawn.x, 0, posToSpawn.z + width);
                break;

            case 4:
                Debug.Log("west");
                posToSpawn = new Vector3(posToSpawn.x, 0, posToSpawn.z - width);
                break;

            default:
                Debug.LogError("Something Went Wrong");
                break;
        }

        for (int i = 0; i < spawnedPositions.Count; i++)
        {
            if (posToSpawn == spawnedPositions[i])
            {
                Debug.Log("Cannot Spawn");
                // just open door
                return;
            }
        }

        Instantiate(arenaToSpawn, posToSpawn, transform.localRotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(posToSpawn, new Vector3(199.9f,50,199.9f));
    }
}
