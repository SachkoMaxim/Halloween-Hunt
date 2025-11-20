using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [Header("References")]
    public ScrollRect scrollRect;
    public RectTransform content;
    public GameObject levelBar;

    private Image[] dots;
    private int currentPage = 0;
    private int totalPages = 0;
    private float targetValue = 0f;
    private Color activeColor = Color.yellow;
    private Color inactiveColor = Color.gray;

    void Awake()
    {
        totalPages = content.childCount;
        dots = levelBar.GetComponentsInChildren<Image>();
    }

    void Start()
    {
        UpdateDots(0);
    }

    public void BackToMain()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }

    public void Next()
    {
        if (currentPage < totalPages - 1)
        {
            currentPage++;
            UpdatePage();
        }
    }

    public void Previous()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    public void OpenLevel(int levelId)
    {
        string levelName = "Level " + levelId;
        SceneManager.LoadSceneAsync(levelName);
    }

    private void UpdatePage()
    {
        float step = 1f / (totalPages - 1);
        targetValue = step * currentPage;
        scrollRect.horizontalNormalizedPosition = targetValue;
        UpdateDots(currentPage);
    }

    private void UpdateDots(int page)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].color = (i == page) ? activeColor : inactiveColor;
        }
    }
}
