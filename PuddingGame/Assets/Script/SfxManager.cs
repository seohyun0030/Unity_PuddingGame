using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager i;

    AudioSource audioSource;
    public AudioClip[] clips;
    public float[] volume;

    private void Awake()
    {
        i = this;
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string type)
    {
        int index = 0;

        switch (type)
        {
            case "Jump": index = 0; break;
            case "Collision": index = 1; break;
            case "Death": index = 2; break;
            case "GetItem": index = 3; break;
            case "UseItem": index = 4; break;
            case "ButtonTouch": index = 5; break;
            case "SavePoint": index = 6; break;
            case "Chocolate": index = 7; break;
            case "IcicleFalling": index = 8; break;
            case "IcicleDropped": index = 9; break;
            case "Portal": index = 10; break;
        }

        audioSource.clip = clips[index];
        audioSource.volume = volume[index];
        audioSource.PlayOneShot(clips[index]);
    }
}
