using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject gameOverScreen;

    void Start()
    {
        Player.OnPlayerDied += GameOverScreen;
        gameOverScreen.SetActive(false);
    }

    private void OnDestroy()
    {
        Player.OnPlayerDied -= GameOverScreen;
    }

    void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResetLevel()
    {
        gameOverScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
