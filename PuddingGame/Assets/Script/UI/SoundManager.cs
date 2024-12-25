using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SoundManager : MonoBehaviour
{
    public Slider BGMSlider;
    public Slider SFXSlider;
    [SerializeField] private AudioMixer audioMixer;

    private float[] audioVolumes = new float[3];
    private void Awake()
    {   
      BGMSlider.onValueChanged.AddListener(ChangeBGM);
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            BGMSlider.value = PlayerPrefs.GetFloat("Volume");
        }
        else
            BGMSlider.value = 0.5f;

        if (PlayerPrefs.HasKey("Volume_2"))
        {
            SFXSlider.value = PlayerPrefs.GetFloat("Volume_2");
        }
        else
            SFXSlider.value = 0.5f;

        audioMixer.SetFloat("BGM", Mathf.Log10(BGMSlider.value) * 20);
        audioMixer.SetFloat("BGM", Mathf.Log10(SFXSlider.value) * 20);
    }


    public void ChangeBGM(float volume)
    {
        audioMixer.SetFloat("BGM" , Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume", BGMSlider.value);
    }

    public void ChangeSFX(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume_2", SFXSlider.value);
    }
}

