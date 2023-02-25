using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject OptionsMenuUI;
    //Update kollar om någon trycker på esc.
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    //resume tar bort pauseUI och startar tiden igen
    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        pauseoptions();
    }
    //pauseoptions gör egentligen bara så att när spelaren trycker på esc kommer man tillbaka till pausemenu
    public void pauseoptions()
    {
        PauseMenuUI.SetActive(false);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OptionsMenuUI.SetActive(false);
            PauseMenuUI.SetActive(true);
        }
    }
    //main menu grejer
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void quit()
    {
        Application.Quit();
    }
}
