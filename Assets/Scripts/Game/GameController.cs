using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    void Start()
    {
        Player.OnPlayerDied += GameOverScreen;
        gameOverScreen.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            PauseScreen();
        }
    }

    private void OnDestroy()
    {
        Player.OnPlayerDied -= GameOverScreen;
    }

    void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        InputBlocker.Blocked = true;
        Time.timeScale = 0;
    }

    public void PauseScreen()
    {
        pauseScreen.SetActive(true);
        InputBlocker.Blocked = true;
        Time.timeScale = 0;
    }

    public void ContinueLevel()
    {
        pauseScreen.SetActive(false);
        InputBlocker.Blocked = false;
        Time.timeScale = 1;
    }

    public void ResetLevel()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        InputBlocker.Blocked = false;
        Time.timeScale = 1;
    }
}
