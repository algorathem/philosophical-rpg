using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;  // Reference to the AudioMixer component

    public void SetMasterVolume(float volume)
    {
        // Set the master volume to the provided value
        audioMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20f);
    }

    public void SetMusicVolume(float volume)
    {
        // Set the music volume to the provided value
        audioMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20f);
    }

    public void SetSFXVolume(float volume)
    {
        // Set the SFX volume to the provided value
        audioMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20f);
    }
}
