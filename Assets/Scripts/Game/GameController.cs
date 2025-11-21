using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject levelIntro;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public GameObject winScreen;
    public Animator transitionAnim;

    private List<Enemy> enemies = new List<Enemy>();
    private static bool levelIntroShown = false;

    void Awake()
    {
        Time.timeScale = 1;
        enemies.AddRange(FindObjectsOfType<Enemy>());
        transitionAnim.gameObject.SetActive(true);
        transitionAnim.speed = 1f;
        StartCoroutine(BeginningTransition());
    }

    void Start()
    {
        Player.OnPlayerDied += GameOverScreen;
        gameOverScreen.SetActive(false);

        if (!levelIntroShown)
        {
            StartCoroutine(ShowLevelIntro());
            levelIntroShown = true;
        }
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
        levelIntroShown = false;
        StartCoroutine(SceneTransition("Level Select"));
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
        StartCoroutine(SceneTransition(SceneManager.GetActiveScene().name));
    }

    public void Next()
    {
        levelIntroShown = false;
        StartCoroutine(SceneTransition(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void EnemyDied(Enemy e)
    {
        enemies.Remove(e);

        if (enemies.Count == 0)
        {
            UnlockNextLevel();
            WinScreen();
        }
    }

    private void UnlockNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    private void WinScreen()
    {
        winScreen.SetActive(true);
        InputBlocker.Blocked = true;
        Time.timeScale = 0;
    }

    private IEnumerator ShowLevelIntro()
    {
        levelIntro.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        levelIntro.SetActive(false);
    }

    private IEnumerator BeginningTransition()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1f);

        InputBlocker.Blocked = false;
        Time.timeScale = 1;
    }

    private IEnumerator SceneTransition(string sceneName)
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSecondsRealtime(1f);

        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    private IEnumerator SceneTransition(int sceneIndex)
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSecondsRealtime(1f);

        yield return SceneManager.LoadSceneAsync(sceneIndex);
    }
}
