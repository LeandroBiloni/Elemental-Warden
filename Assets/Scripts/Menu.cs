using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject subExit;
    public GameObject controlsSubmenu;
    public GameObject mainMenu;
    public GameObject creditsSubmenu;
    public GameObject difficultySubMenu;
    public GameObject memoryObj;
    public Memory memory;

    private void Start()
    {
        memoryObj = GameObject.Find("Memory");
        memory = memoryObj.GetComponent<Memory>();
    }

    public void Play()
    {
        difficultySubMenu.SetActive(true);
    }

    public void Easy()
    {
        memory.difficulty = 1;
        SceneManager.LoadScene("Tutorial");
    }

    public void Normal()
    {
        memory.difficulty = 2;
        SceneManager.LoadScene("Tutorial");
    }

    public void Hard()
    {
        memory.difficulty = 3;
        SceneManager.LoadScene("Tutorial"); ;
    }

    public void ExitDifficulty()
    {
        difficultySubMenu.SetActive(false);
    }

    public void Controls()
    {
        controlsSubmenu.SetActive(true);
    }

    public void ExitControls()
    {
        controlsSubmenu.SetActive(false);
    }

    public void Credits()
    {
        creditsSubmenu.SetActive(true);
    }

    public void ExitCredits()
    {
        creditsSubmenu.SetActive(false);
    }

    public void ShowSubMenu()
    {
        subExit.SetActive(true);
    }

    public void HideSubMenu()
    {
        subExit.SetActive(false);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Replay()
    {
        SceneManager.LoadScene("WaterRoom");
    }

    public void ExitGame()
    {
        Debug.Log("Sali del juego");
        Application.Quit();
    }
}
