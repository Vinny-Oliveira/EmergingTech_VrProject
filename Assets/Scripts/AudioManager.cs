using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

    public AudioSource audioButton;
    public List<AudioClip> listAudioClips = new List<AudioClip>();

    public static AudioManager instance;

    private void Awake() {
        instance = this;
    }

    /// <summary>
    /// Play the sound of a button
    /// </summary>
    public void PlayButtonSound() {
        if (listAudioClips.Count < 1) {
            return;
        }

        int rand = Random.Range(0, listAudioClips.Count);
        audioButton.clip = listAudioClips[rand];

        audioButton.Play();
    }
}
