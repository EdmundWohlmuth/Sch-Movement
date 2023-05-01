using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public static Camera mainCam;
    public List<WeaponData> weaponType = new List<WeaponData>();
    public WeaponController playerWeapons;

    public TextMeshProUGUI ammoText;

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
        //ScreenState();
        ammoText.text = "Ammo: " + playerWeapons.ammo;
    }

    void ScreenState()
    {
        switch (UIManager.uIManager.currentState)
        {
            case UIManager.CurrentScreen._MainMenu:

                break;

            case UIManager.CurrentScreen._GamePlay:
                
                break;

            case UIManager.CurrentScreen._Pause:

                break;

            case UIManager.CurrentScreen._Win:

                break;

            case UIManager.CurrentScreen._Loose:

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

    }
}
