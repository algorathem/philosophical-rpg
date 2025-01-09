using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20);
        PlayerPrefs.SetFloat("musicVolume", Mathf.Log10(level) * 20);
    }


    public void SetSFXVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20);
        PlayerPrefs.SetFloat("soundFXVolume", Mathf.Log10(level) * 20);
    }

    public void Update()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
