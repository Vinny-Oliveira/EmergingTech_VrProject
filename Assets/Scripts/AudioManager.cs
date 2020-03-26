using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

    public AudioSource audioButton;

    public static AudioManager instance;

    private void Awake() {
        instance = this;
    }

    /// <summary>
    /// Play the sound of a button
    /// </summary>
    public void PlayButtonSound() {
        audioButton.Play();
    }
}
