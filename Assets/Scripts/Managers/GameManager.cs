using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public static Camera mainCam;
    public ArenaManager arenaManager;

    public static List<AIController> currentEnemies = new List<AIController>();
    public static List<GameObject> droppedWeapons = new List<GameObject>();
    // DO THIS WITH BULLETS TOO - WITH 50 GUYS SHOOTING BUCKSHOT CAN USE THIS TO REDUCE LAG

    public List<WeaponData> weaponType = new List<WeaponData>();
    public WeaponController playerWeapons;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI enemyCountText;

    public List<ArenaManager> arenaToSpawn = new List<ArenaManager>();

    // ARENAS
    public Dictionary<Vector3Int, ArenaManager> spawnedPositions = new Dictionary<Vector3Int, ArenaManager>();
    public List<GameObject> allDoors = new List<GameObject>();
    public Vector3Int posToSpawn;
    public int arenasCompleted;
    public Material openableMat;
    public Material nonOpenableMat;

    [ContextMenu("KillAll")]
    public void KillAllEnemies()
    {
        for (int i = currentEnemies.Count - 1; i >= 0; i--)
        {
            currentEnemies[i].gameObject.GetComponent<HealthController>().TakeDamage(int.MaxValue);
        }

        Debug.Log(currentEnemies.Count);
    }
    private void Awake()
    {
        mainCam = Camera.main;

        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (gameManager != this && gameManager != null)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ScreenState();
        if (currentEnemies.Count <= 3)
        {
            // highlight enemies so they're easier to find
        }
    }

    void ScreenState()
    {
        switch (UIManager.uIManager.currentState)
        {
            case UIManager.CurrentScreen._MainMenu:
                UIManager.uIManager.MainMenuState(); // THESE ARE CALLED EVERY FRAME, BAD.
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Confined;

                break;

            case UIManager.CurrentScreen._GamePlay:

                UIManager.uIManager.GamePlayState();
                Time.timeScale = 1; // SHOULD ONLY BE CALLED WHEN STATE IS CHANGED
                Cursor.lockState = CursorLockMode.Locked;

                if (playerWeapons == null && PlayerController.instance != null) playerWeapons = PlayerController.instance.GetComponent<WeaponController>();
                //if (enemySpawner == null) enemySpawner = GameObject.Find("LevelPrefab").GetComponent<EnemySpawner>(); //TEMP

                if (playerWeapons)
                    ammoText.text = "Ammo: " + playerWeapons.ammo;
                enemyCountText.text = "Enemies: " + currentEnemies.Count;

                if (currentEnemies.Count == 0)
                {
                    //Debug.Log("All Dead");
                    Door.SetDoorState(true);
                }
                break;

            case UIManager.CurrentScreen._Pause:

                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;

                break;

            case UIManager.CurrentScreen._Win:

                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;

                break;

            case UIManager.CurrentScreen._Loose:

                UIManager.uIManager.LooseState();
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
                UIManager.uIManager.LooseState();

                break;

            case UIManager.CurrentScreen._SaveGame:

                break;

            case UIManager.CurrentScreen._NewGame:

                break;

            default:
                break;
        }
    }

    public void GameOver()
    {
        UIManager.uIManager.currentState = UIManager.CurrentScreen._Loose;

        if (currentEnemies.Count > 0 || droppedWeapons.Count > 0) Reset();
    }

    void Reset()
    {
        currentEnemies.Clear();
        droppedWeapons.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawCube(posToSpawn, new Vector3(199.9f, 50f, 199.9f));
    }
}
