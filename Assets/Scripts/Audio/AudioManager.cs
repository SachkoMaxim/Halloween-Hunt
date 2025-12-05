using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;
    [SerializeField] public AudioClip background;
    [SerializeField] public AudioMixer gameMixer;
    [SerializeField] public AudioSource SFXObject;

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
        gameMixer.SetFloat("SFXVolume", Mathf.Log10(GetSFXVolume()) * 20f);
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

    public void SetSFXVolume(float level)
    {
        gameMixer.SetFloat("SFXVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("SFXVolumeKey", level);
        PlayerPrefs.Save();
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.HasKey("SFXVolumeKey") ? PlayerPrefs.GetFloat("SFXVolumeKey") : 1.0f;
    }

    public void PlaySFX(AudioClip audioClip)
    {
        SFXSource.PlayOneShot(audioClip);
    }

    public void PlaySFXClip(AudioClip audioClip, Transform spawnPosition)
    {
        AudioSource audioSource = Instantiate(SFXObject, spawnPosition.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.Play();

        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
}
