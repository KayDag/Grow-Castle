using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource sfxSource;
    public AudioSource musicSource;
    public float musicVolume = 0.4f;
    public float sfxVolume = 0.7f;

    public AudioClip checkpoint;
    public AudioClip collectGold;
    public AudioClip click;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip bgm;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    void Start()
    {
        musicSource.clip = bgm;
        musicSource.loop = true;
        musicSource.Play();
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    public void PlayClick()
    {
        sfxSource.PlayOneShot(click);
    }
    public void PlayCollectGold()
    {
        sfxSource.PlayOneShot(collectGold);
    }
    public void PlayReachCheckPoint()
    {
        sfxSource.PlayOneShot(checkpoint);
    }
    public void PlayWin()
    {
        sfxSource.PlayOneShot(win);
    }
    public void PlayLose()
    {
        sfxSource.PlayOneShot(lose);
    }
}
