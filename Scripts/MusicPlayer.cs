using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private GameObject HP;

    public AudioClip[] musicClips;
    public AudioClip backgroundMusicClip;
    private AudioSource musicSource;
    private AudioSource backgroundSource;

    [SerializeField] ParticleSystem particleSystem;

    private int musicIndex;
    private void Start()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        backgroundSource = gameObject.AddComponent<AudioSource>();

       
        PlayBackgroundMusic();
    }

    private void Update()
    {
        if (!musicSource.isPlaying&& particleSystem.isPlaying)
        {
            // ¸èÇú²¥·ÅÍê±Ï
            Array.Copy(musicClips, musicIndex + 1, musicClips, musicIndex, musicClips.Length - musicIndex - 1);
            Array.Resize(ref musicClips, musicClips.Length - 1);

            //Debug.Log("¸èÇú²¥·ÅÍê±Ï");
            PlayRandomMusic();
        }
        if (musicSource.isPlaying)
        {
            float musicVolume = HP.GetComponent<Image>().material.GetFloat("_value");
            float backgroundVolume = 1 - musicVolume;
            musicSource.volume = musicVolume;
            backgroundSource.volume = backgroundVolume;
        }else
            backgroundSource.volume =1-HP.GetComponent<Image>().material.GetFloat("_value");
    }

    private void PlayRandomMusic()
    {
        if (musicClips.Length > 0)
        {
            musicIndex = UnityEngine.Random.Range(0, musicClips.Length);
            AudioClip randomClip = musicClips[musicIndex];

            musicSource.clip = randomClip;
            musicSource.Play();
        }
    }

    private void PlayBackgroundMusic()
    {
        backgroundSource.clip = backgroundMusicClip;
        backgroundSource.loop = true;
        backgroundSource.Play();
    }

    public void PlayMusic()
    {
        musicIndex = 0;
        musicSource.clip = musicClips[musicIndex];
    }
}
