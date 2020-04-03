using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueManager : MonoBehaviour {

    [Header("Control Variables")]
    public Transform playerCamera;
    public int intWaitTime;
    public const float TURN_AROUND_ANGLE = 120f;

    [Header("Audio Source and Clips")]
    public AudioSource audioSource;
    public AudioClip dialogueStart;
    public AudioClip dialogueTurnAround;
    public AudioClip dialogueFacePanel;
    public AudioClip dialogueBackToWork;
    public AudioClip dialogueSayHoldOn;
    public AudioClip dialogueCantSpeak;
    public AudioClip dialogueSoundAlarm;
    public AudioClip dialogueDoorKnock1;
    public AudioClip dialogueDoorKnock2;
    public AudioClip dialoguePressDoorButton;

    private void Start() {
        // Play the initial dialogue chain
        StartCoroutine(PlayDialogueChain());
    }

    /// <summary>
    /// Tell the player to turn around if they have not done so already
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayDialogueChain() {
        // Set initial rotation
        float initialRotation = playerCamera.localRotation.eulerAngles.y;

        // Play the dialoque that starts the game
        yield return StartCoroutine(PlayEntireDialogue(dialogueStart));

        // Check if player has turned around to face the button panel
        while (!(playerCamera.localRotation.eulerAngles.y > initialRotation + TURN_AROUND_ANGLE && 
                 playerCamera.localRotation.eulerAngles.y < initialRotation + 360f - TURN_AROUND_ANGLE)) {
            yield return StartCoroutine(PlayEntireDialogue(dialogueTurnAround));
        }

        // Play the following dialogue chain after the player has turned around
        yield return StartCoroutine(PlayEntireDialogue(dialogueFacePanel));
        yield return StartCoroutine(PlayEntireDialogue(dialogueBackToWork));
        yield return StartCoroutine(PlayEntireDialogue(dialogueSayHoldOn));
        yield return StartCoroutine(PlayEntireDialogue(dialogueCantSpeak));
    }

    /// <summary>
    /// Play a dialogue audio clip
    /// </summary>
    /// <param name="audioClip"></param>
    void PlayDialogue(AudioClip audioClip) {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    /// <summary>
    /// Play a dialogue until it finishes
    /// </summary>
    /// <param name="audioClip"></param>
    /// <returns></returns>
    IEnumerator PlayEntireDialogue(AudioClip audioClip) {
        PlayDialogue(audioClip);
        yield return new WaitForSeconds(audioClip.length + intWaitTime);
    }
}
