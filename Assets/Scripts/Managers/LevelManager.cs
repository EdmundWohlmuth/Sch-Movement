using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;

    public List<GameObject> northDoors;
    public List<GameObject> southDoors;
    public List<GameObject> eastDoors;
    public List<GameObject> westDoors;

    // Start is called before the first frame update
    void Start()
    {
        if (levelManager == null)
        {
            levelManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (levelManager != this && levelManager != null)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoToGamePlay()
    {
        UIManager.uIManager.currentState = UIManager.CurrentScreen._GamePlay;
        SceneManager.LoadScene("SampleScene");
        AudioManager.audioManager.StartMusic();
    }
    public void GoToMainMenu()
    {
        UIManager.uIManager.currentState = UIManager.CurrentScreen._MainMenu;
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
