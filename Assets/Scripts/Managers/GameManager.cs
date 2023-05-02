using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public static Camera mainCam;

    public static List<GameObject> currentEnemies = new List<GameObject>();
    // DO THIS WITH BULLETS TOO - WITH 50 GUYS SHOOTING BUCKSHOT CAN USE THIS TO REDUCE LAG

    public List<WeaponData> weaponType = new List<WeaponData>();
    public WeaponController playerWeapons;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI enemyCountText;

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
        /*for (int i = 0; i < currentEnemies.Count; i++)
        {
            Debug.Log(currentEnemies[i]);
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {
        ScreenState();

    }

    void ScreenState()
    {
        switch (UIManager.uIManager.currentState)
        {
            case UIManager.CurrentScreen._MainMenu:
                UIManager.uIManager.MainMenuState();
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Confined;

                break;

            case UIManager.CurrentScreen._GamePlay:
              
                UIManager.uIManager.GamePlayState();
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;

                if (playerWeapons == null) playerWeapons = GameObject.Find("Player").GetComponent<WeaponController>();
                ammoText.text = "Ammo: " + playerWeapons.ammo;
                enemyCountText.text = "Enemies: " + currentEnemies.Count;

                if (currentEnemies.Count == 0)
                {
                    //Debug.Log("All Dead");
                    // spawn more
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
    }
}
