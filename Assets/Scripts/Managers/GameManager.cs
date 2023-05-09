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

    public static List<GameObject> currentEnemies = new List<GameObject>();
    public static List<GameObject> droppedWeapons = new List<GameObject>();
    // DO THIS WITH BULLETS TOO - WITH 50 GUYS SHOOTING BUCKSHOT CAN USE THIS TO REDUCE LAG

    public List<WeaponData> weaponType = new List<WeaponData>();
    public WeaponController playerWeapons;
    public EnemySpawner enemySpawner;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI enemyCountText;

    public List<Vector3> spawnedPositions = new List<Vector3>();
    public Vector3 posToSpawn;
    int arenasCompleted;

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

        //TEMP
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            arenaManager.SpawnArena(1);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            arenaManager.SpawnArena(2);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            arenaManager.SpawnArena(3);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            arenaManager.SpawnArena(4);
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

                if (playerWeapons == null) playerWeapons = GameObject.Find("Player").GetComponent<WeaponController>();
                if (enemySpawner == null) enemySpawner = GameObject.Find("LevelPrefab").GetComponent<EnemySpawner>(); //TEMP

                ammoText.text = "Ammo: " + playerWeapons.ammo;
                enemyCountText.text = "Enemies: " + currentEnemies.Count;

                if (currentEnemies.Count == 0)
                {
                    //Debug.Log("All Dead");
                    // spawn more
                    enemySpawner.BeginSpawn();
                    arenaManager.SetOpenable();
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
}
