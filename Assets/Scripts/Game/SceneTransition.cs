using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Refences")]
    [SerializeField] public Animator transitionAnim;

    void Awake()
    {
        transitionAnim.gameObject.SetActive(true);
        transitionAnim.speed = 1f;
    }

    public IEnumerator BeginningTransition()
    {
        Time.timeScale = 0;
        transitionAnim.SetTrigger("start");

        yield return new WaitForSecondsRealtime(2f);

        InputBlocker.Blocked = false;
        Time.timeScale = 1;
    }

    public IEnumerator EndTransition(string sceneName)
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSecondsRealtime(1f);

        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    public IEnumerator EndTransition(int sceneIndex)
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSecondsRealtime(1f);

        yield return SceneManager.LoadSceneAsync(sceneIndex);
    }
}
