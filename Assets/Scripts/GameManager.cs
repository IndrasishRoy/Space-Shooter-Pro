using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool _isGameOver;
    [SerializeField]
    private GameObject _pauseMenuPanel;
    private Animator _pauseAnimator;

    private void Start()
    {
        _pauseAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1);//Current Game Scene
        }
        //GameOver();
        //if the escape is pressed
        //quit the application
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Application.Quit();
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            
            _pauseMenuPanel.SetActive(true);
            _pauseAnimator.SetBool("isPause", true);
            Time.timeScale = 0;
        }
    }
    public void GameOver()
    {
        _isGameOver = true;
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(2);//Current Game Scene
        }
    }
    public void ResumeGame()
    {
        
        _pauseMenuPanel.SetActive(false);
        _pauseAnimator.SetBool("isPause", false);
        Time.timeScale = 1f;
    }
}
