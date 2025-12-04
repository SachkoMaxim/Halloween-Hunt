using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioClip background;
    [SerializeField] public AudioMixer gameMixer;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance.background != this.background)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        musicSource.clip = background;
    }

    void Start()
    {
        gameMixer.SetFloat("musicVolume", Mathf.Log10(GetMusicVolume()) * 20f);
        musicSource.Play();
    }

    public void SetMusicVolume(float level)
    {
        gameMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("MusicVolumeKey", level);
        PlayerPrefs.Save();
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.HasKey("MusicVolumeKey") ? PlayerPrefs.GetFloat("MusicVolumeKey") : 1.0f;
    }
}
