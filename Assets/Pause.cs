using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    // Start is called before the first frame update

    public static bool isPaused = false;
    [SerializeField] GameObject pauseMenu;

    [SerializeField] GameObject player;



    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }


    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        player.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        player.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    public void PlayGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        player.SetActive(true);
        Cursor.visible = false;
        SceneManager.LoadScene("Playground");
    }

    public void Exit()
    {
        Application.Quit();
    }

}
