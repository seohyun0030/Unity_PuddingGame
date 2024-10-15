using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicsource;

    public AudioSource sfxsource;

    public void SetMusicVolume(float volume)
    {
        musicsource.volume = volume;
    }
    public void SetSfxVolume(float volume)
    {
        sfxsource.volume = volume;
    }
}
