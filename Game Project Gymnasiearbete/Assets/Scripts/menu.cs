using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject OptionsMenuUI;
    //Update kollar om n�gon trycker p� esc.
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
    //s�tter p� pauseUI och stannar tiden
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        pauseoptions();
    }
    //pauseoptions g�r egentligen bara s� att n�r spelaren trycker p� esc kommer man tillbaka till pausemenu
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
    //s�tter p� tiden och s�tter antal wins till 0 samt laddar om scenen
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        GameObject gameManager = GameObject.Find("GameManager");
        GameManager wins = gameManager.GetComponent<GameManager>();
        wins.redWins = 0;
        wins.greenWins = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
