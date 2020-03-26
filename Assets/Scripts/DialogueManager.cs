using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueManager : MonoBehaviour {

    public AudioSource audioSource;

    public AudioClip dialogueStart;
    public AudioClip dialogueTurnAround;

    private void Start() {
        // Play the dialoque that starts the game
        PlayDialogue(dialogueStart);
    }

    /// <summary>
    /// Play dialoque to tell player to turn around
    /// </summary>
    public void PlayTurnAround() {
        PlayDialogue(dialogueTurnAround);
    }

    /// <summary>
    /// Play a dialogue audio clip
    /// </summary>
    /// <param name="audioClip"></param>
    void PlayDialogue(AudioClip audioClip) {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
