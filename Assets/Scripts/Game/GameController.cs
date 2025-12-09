using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Refences")]
    [SerializeField] public GameObject levelIntro;
    [SerializeField] public GameObject gameOverScreen;
    [SerializeField] public GameObject pauseScreen;
    [SerializeField] public GameObject winScreen;
    [SerializeField] public Animator transitionAnim;

    [Header("Audio Clips")]
    [SerializeField] protected AudioClip ambience;
    [SerializeField] protected AudioClip pauseClip;
    [SerializeField] protected AudioClip looseClip;
    [SerializeField] protected AudioClip winClip;

    private List<Enemy> enemies = new List<Enemy>();
    private bool canOpen = true;
    private static bool levelIntroShown = false;

    void Awake()
    {
        enemies.AddRange(FindObjectsOfType<Enemy>());
        transitionAnim.gameObject.SetActive(true);
        transitionAnim.speed = 1f;
    }

    void Start()
    {
        StartCoroutine(BeginningTransition());
        Player.OnPlayerDied += GameOverScreen;
        gameOverScreen.SetActive(false);
        AudioManager.instance.PlayAmbience(ambience);

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
            PauseScreen(canOpen);
        }
    }

    private void OnDestroy()
    {
        Player.OnPlayerDied -= GameOverScreen;
        AudioManager.instance?.StopAmbience();
    }

    public void GameOverScreen()
    {
        AudioManager.instance.PlaySFXClip(looseClip, transform);
        gameOverScreen.SetActive(true);
        InputBlocker.Blocked = true;
        Time.timeScale = 0;
    }

    public void PauseScreen(bool isActive)
    {
        AudioManager.instance.PlaySFXClip(pauseClip, transform);
        pauseScreen.SetActive(isActive);
        EventSystem.current.SetSelectedGameObject(null);
        InputBlocker.Blocked = isActive;
        Time.timeScale = isActive ? 0 : 1;
        canOpen = !canOpen;
    }

    public void BackHome()
    {
        levelIntroShown = false;
        StartCoroutine(SceneTransition("Level Select"));
    }

    public void Continue()
    {
        PauseScreen(false);
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

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextScene < 0 || nextScene >= SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(SceneTransition("Level Select"));
        }
        else
        {
            StartCoroutine(SceneTransition(nextScene));
        }
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
        AudioManager.instance.PlaySFXClip(winClip, transform);
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
