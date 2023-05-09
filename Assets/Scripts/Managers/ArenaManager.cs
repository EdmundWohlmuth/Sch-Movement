using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    int width = 200;
    public GameObject door;
    public List<GameObject> doors;
    public Material openableMat;

    public int lastOpenDoor;

    public GameObject arenaToSpawn;

    private void Awake()
    {
        GameManager.gameManager.spawnedPositions.Add(gameObject.transform.position);
    }

    // Start is called before the first frame update
    void Start()
    {
        doors.Clear();
        foreach (Transform door in door.transform)
        {
            doors.Add(door.gameObject);
        }

        GameManager.gameManager.arenaManager = gameObject.GetComponent<ArenaManager>();

        if (lastOpenDoor == 0) return;

        switch (lastOpenDoor)
        {
            case 1:
                Debug.Log("south");

                doors[1].SetActive(false);
                doors[0].SetActive(true);
                doors[2].SetActive(true);
                doors[3].SetActive(true);
                break;

            case 2:
                Debug.Log("north");

                doors[0].SetActive(false);
                doors[1].SetActive(true);
                doors[2].SetActive(true);
                doors[3].SetActive(true);

                break;

            case 3:
                Debug.Log("west");
                doors[3].SetActive(false);
                doors[1].SetActive(true);
                doors[0].SetActive(true);
                doors[2].SetActive(true);

                break;

            case 4:
                Debug.Log("east");

                doors[2].SetActive(false);
                doors[1].SetActive(true);
                doors[3].SetActive(true);
                doors[0].SetActive(true);
                break;

            default:
                Debug.LogError("Something Went Wrong");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOpenable()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].gameObject.layer = 9;
            doors[i].gameObject.GetComponent<Renderer>().material = openableMat;
        }
    }

    public void SpawnArena(int chosenDirection)
    {
        switch (chosenDirection)
        {
            case 1:
                Debug.Log("north");
                GameManager.gameManager.posToSpawn = new Vector3(GameManager.gameManager.posToSpawn.x + width, 0, GameManager.gameManager.posToSpawn.z);
                doors[0].SetActive(false);
                break;

            case 2:
                Debug.Log("south");
                GameManager.gameManager.posToSpawn = new Vector3(GameManager.gameManager.posToSpawn.x - width, 0, GameManager.gameManager.posToSpawn.z);
                doors[1].SetActive(false);

                break;

            case 3:
                Debug.Log("east");
                doors[2].SetActive(false);
                GameManager.gameManager.posToSpawn = new Vector3(GameManager.gameManager.posToSpawn.x, 0, GameManager.gameManager.posToSpawn.z + width);
                break;

            case 4:
                Debug.Log("west");
                GameManager.gameManager.posToSpawn = new Vector3(GameManager.gameManager.posToSpawn.x, 0,
                    GameManager.gameManager.posToSpawn.z - width);
                doors[3].SetActive(false);
                break;

            default:
                Debug.LogError("Something Went Wrong");
                break;
        }

        for (int i = 0; i < GameManager.gameManager.spawnedPositions.Count; i++)
        {
            if (GameManager.gameManager.posToSpawn == GameManager.gameManager.spawnedPositions[i])
            {
                Debug.Log("Cannot Spawn");
                return;
            }
        }

        GameObject newArena = Instantiate(arenaToSpawn, GameManager.gameManager.posToSpawn, transform.localRotation);
        GameManager.gameManager.arenaManager = newArena.GetComponent<ArenaManager>();
        GameManager.gameManager.arenaManager.GetComponent<ArenaManager>().lastOpenDoor = chosenDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(GameManager.gameManager.posToSpawn, new Vector3(199.9f,50,199.9f));
    }
}
