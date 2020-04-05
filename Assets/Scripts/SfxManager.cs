using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SfxManager : SingletonManager<SfxManager> {

    public AudioSource audioSource;
    public List<AudioClip> listAudioClips = new List<AudioClip>();

    public AudioClip audioExplosion;
    public AudioClip audioScreams;
    public AudioClip audioAlarm;

    /// <summary>
    /// Play a given audio clip
    /// </summary>
    /// <param name="audioClip"></param>
    public void PlaySfx(AudioClip audioClip) {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    /// <summary>
    /// Play the sound of a button
    /// </summary>
    public void PlayButtonSound() {
        if (listAudioClips.Count < 1) {
            return;
        }

        int rand = Random.Range(0, listAudioClips.Count);
        audioSource.clip = listAudioClips[rand];

        audioSource.Play();
    }
}
