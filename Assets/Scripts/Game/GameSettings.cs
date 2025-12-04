using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [Header("Menu Settings")]
    [SerializeField] public Slider musicSlider;

    private AudioManager gameAudioManager;

    public void Initialize(AudioManager audioManager)
    {
        gameAudioManager = audioManager;
    }

    void Start()
    {
        musicSlider.value = gameAudioManager.GetMusicVolume();
    }

    void OnDestroy()
    {
        InputBlocker.Interactible = true;
    }

    public void SetMusic(float level)
    {
        gameAudioManager.SetMusicVolume(level);
    }

    public void CloseSettings()
    {
        Destroy(gameObject);
    }
}
