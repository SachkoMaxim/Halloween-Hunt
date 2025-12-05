using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [Header("Menu Settings")]
    [SerializeField] public Slider musicSlider;
    [SerializeField] public Slider SFXSlider;

    private AudioManager gameAudioManager;

    public void Initialize(AudioManager audioManager)
    {
        gameAudioManager = audioManager;
    }

    void Start()
    {
        musicSlider.value = gameAudioManager.GetMusicVolume();
        SFXSlider.value = gameAudioManager.GetSFXVolume();
    }

    void OnDestroy()
    {
        InputBlocker.Interactible = true;
    }

    public void SetMusic(float level)
    {
        gameAudioManager.SetMusicVolume(level);
    }

    public void SetSFX(float level)
    {
        gameAudioManager.SetSFXVolume(level);
    }

    public void CloseSettings()
    {
        Destroy(gameObject);
    }
}
