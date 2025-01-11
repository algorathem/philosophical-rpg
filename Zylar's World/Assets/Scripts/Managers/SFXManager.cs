using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;  // Static reference to the SFXManager instance
    [SerializeField] private AudioSource sfxAudioSource;  // Reference to the AudioSource component

    private void Awake()
    {
        // Check if an instance of the SFXManager already exists
        if (instance == null)
        {
            // If not, set the instance to this object
            instance = this;
        }
        else
        {
            // If an instance already exists, destroy this object
            Destroy(gameObject);

            // Return to prevent any further code execution
            return;

        }
    }

    public void PlaySFXClip(AudioClip clip, Transform spawnTransform, float volume)
    {
        // Check if the clip is null
        if (clip == null)
        {
            // If so, log a warning and return
            Debug.LogWarning("SFXManager: PlaySFXClip() - AudioClip is null!");
            return;
        }

        // Create a new GameObject to play the sound effect
        AudioSource audioSource = Instantiate(sfxAudioSource, spawnTransform.position, Quaternion.identity);

        // Set the AudioClip to the provided clip
        audioSource.clip = clip;

        // Set the volume of the audio source
        audioSource.volume = volume;

        // Play the audio clip
        audioSource.Play();

        // Get the length of the audio clip
        float clipLength = clip.length;

        // Destroy the GameObject after the audio clip has finished playing
        Destroy(audioSource.gameObject, clip.length);
    }
}
