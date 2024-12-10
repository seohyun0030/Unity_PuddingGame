using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum EAudioMixerType { Master, BGM, SFX }
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioMixer audioMixer;

    private float[] audioVolumes = new float[3];
    private void Awake()
    {
        Instance = this;
    }

    public void SetAudioVolume(EAudioMixerType audioMixerType, float volume)
    {
        audioMixer.SetFloat(audioMixerType.ToString(), Mathf.Log10(volume) * 20);
    }

    public void ChangeBGM(float volume)
    {
        SoundManager.Instance.SetAudioVolume(EAudioMixerType.BGM, volume);
    }

    public void ChangeSFX(float volume)
    {
        SoundManager.Instance.SetAudioVolume(EAudioMixerType.SFX, volume);
    }
}

