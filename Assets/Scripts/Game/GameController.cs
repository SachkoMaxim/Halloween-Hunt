using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public GameObject winScreen;

    private List<Enemy> enemies = new List<Enemy>();

    void Awake()
    {
        Time.timeScale = 1;
        InputBlocker.Blocked = false;
        enemies.AddRange(FindObjectsOfType<Enemy>());
    }

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

    public void GameOverScreen()
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

    public void BackHome()
    {
        SceneManager.LoadSceneAsync("Level Select");
    }

    public void Continue()
    {
        pauseScreen.SetActive(false);
        InputBlocker.Blocked = false;
        Time.timeScale = 1;
    }

    public void Restart()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        winScreen.SetActive(false);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void Next()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void WinScreen()
    {
        winScreen.SetActive(true);
        InputBlocker.Blocked = true;
        Time.timeScale = 0;
    }

    public void EnemyDied(Enemy e)
    {
        enemies.Remove(e);

        if (enemies.Count == 0)
        {
            WinScreen();
        }
    }
}
