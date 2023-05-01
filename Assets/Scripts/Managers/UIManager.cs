using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager uIManager;

    [Header("User Interface Screens")]
    public Canvas MainMenu;
    public Canvas GamePlay;
    public Canvas Loose;
    public Canvas Save;
    public Canvas Pause;
    public Canvas NewGame;
    public Canvas Win;

    public enum CurrentScreen
    {
        _MainMenu,
        _GamePlay,
        _Pause,
        _Win,
        _Loose,
        _SaveGame,
        _NewGame
    }
    public CurrentScreen currentState;

    void Start()
    {
        //MainMenuState();
        GamePlayState();
    }

    public void MainMenuState()
    {
        MainMenu.enabled = true;
        GamePlay.enabled = false;
        Loose.enabled = false;
        Save.enabled = false;
        Pause.enabled = false;
        NewGame.enabled = false;
        Win.enabled = false;

        currentState = CurrentScreen._MainMenu;
    }

    public void GamePlayState()
    {
        MainMenu.enabled = false;
        GamePlay.enabled = true;
        Loose.enabled = false;
        Save.enabled = false;
        Pause.enabled = false;
        NewGame.enabled = false;
        Win.enabled = false;

        currentState = CurrentScreen._GamePlay;
    }

    public void LooseState()
    {
        MainMenu.enabled = false;
        GamePlay.enabled = false;
        Loose.enabled = true;
        Save.enabled = false;
        Pause.enabled = false;
        NewGame.enabled = false;
        Win.enabled = false;

        currentState = CurrentScreen._Loose;
    }

    public void SaveState()
    {
        Save.enabled = true;

        currentState = CurrentScreen._SaveGame;
    }

    public void PauseState()
    {
        MainMenu.enabled = false;
        GamePlay.enabled = true;
        Loose.enabled = false;
        Save.enabled = false;
        Pause.enabled = true;
        NewGame.enabled = false;
        Win.enabled = false;

        currentState = CurrentScreen._Pause;
    }

    public void NewGameState()
    {
        MainMenu.enabled = false;
        GamePlay.enabled = false;
        Loose.enabled = false;
        Save.enabled = false;
        Pause.enabled = false;
        NewGame.enabled = true;
        Win.enabled = false;

        currentState = CurrentScreen._NewGame;
    }

    public void WinState()
    {
        MainMenu.enabled = false;
        GamePlay.enabled = false;
        Loose.enabled = false;
        Save.enabled = false;
        Pause.enabled = false;
        NewGame.enabled = false;
        Win.enabled = true;

        currentState = CurrentScreen._Win;
    }
}
