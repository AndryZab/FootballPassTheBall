using System;
using System.Collections;
using UnityEngine;

public class stateitem : MonoBehaviour
{
    public GameObject[] backgrounds;
    public AudioSource sourceMusicSave;
    public AudioSource defaultSource;

    public GameObject defaultbackground;

    public AudioClip[] musicClips;
    private int currentMusicIndex;
    private bool isPlaying;
    public bool musicfound = false;
    private timerForEndGame timerForEndGame;
    private void Start()
    {
        LoadBackgroundsState();
        if (timerForEndGame != null && timerForEndGame.playMusic == true)
        {
            LoadMusicState();

        }
        else
        {
            LoadMusicState();
        }
    }

   

    public void LoadMusicState()
    {

        for (int i = 0; i < musicClips.Length; i++)
        {
            string key = "music_" + i;
            int musicIndex = PlayerPrefs.GetInt(key, -1);

            if (musicIndex == i)
            {
                musicfound = true;
                sourceMusicSave.clip = musicClips[i];
                sourceMusicSave.Play();
                break;
            }
        }

        if (!musicfound && defaultSource.clip != null)
        {
            defaultSource.Play();
        }
    }

    private void LoadBackgroundsState()
    {
        bool hasSavedState = false;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (PlayerPrefs.HasKey("background_" + i))
            {
                hasSavedState = true;
                break;
            }
        }

        if (hasSavedState)
        {
            defaultbackground.SetActive(false);
            int activeIndex = -1;
            for (int i = 0; i < backgrounds.Length; i++)
            {
                int savedState = PlayerPrefs.GetInt("background_" + i, -1);
                if (savedState == i)
                {
                    activeIndex = i;
                }
                backgrounds[i].SetActive(savedState == i);
            }

            for (int i = 0; i < backgrounds.Length; i++)
            {
                if (i != activeIndex)
                {
                    backgrounds[i].SetActive(false);
                }
            }
        }
        else
        {
            if (defaultbackground != null)
            {
               
                defaultbackground.SetActive(true);

            }
        }
    }

    private void PlayMusic(int index)
    {
        if (index >= 0 && index < musicClips.Length)
        {
            StopAllMusic();
            sourceMusicSave.clip = musicClips[index];
            sourceMusicSave.Play();
            isPlaying = true;
        }
    }

    private void PlayDefaultMusic()
    {
        StopAllMusic();
        defaultSource.Play();
        isPlaying = true;
    }

    private void StopAllMusic()
    {
        defaultSource.Stop();
        sourceMusicSave.Stop();
        isPlaying = false;
    }

    public void StopMusic()
    {
        StopAllMusic();
    }
}
