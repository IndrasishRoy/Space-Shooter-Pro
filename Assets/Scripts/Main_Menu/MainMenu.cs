using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadMainMenu()
    {
            SceneManager.LoadScene(0);
    }
    public void LoadSinglePlayerGame()
    {
        Debug.Log("Single Player Game Loading...");
        SceneManager.LoadScene(1);//Main Game Scene
    }
    public void LoadCoOpMode()
    {
        Debug.Log("Co-Op Mode Loading...");
        SceneManager.LoadScene(2);//Co-Op Mode Scene
    }
    public void ExitOption()
    {
        Application.Quit();
    }
}
