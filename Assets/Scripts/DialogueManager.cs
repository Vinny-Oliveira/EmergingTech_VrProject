using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueManager : MonoBehaviour {

    [Header("Control Variables")]
    public Transform playerCamera;
    public int dialogueInterval;
    public int waitTimeForAlarm;
    public const float TURN_AROUND_ANGLE = 120f;

    [Header("Audio Source and Clips")]
    public AudioSource audioDialogue;
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

    // Dialogue control
    bool isFirstChainFinished = false;
    bool isWaitingForAlarm = false;

    private void Start() {
        // Play the initial dialogue chain
        StartCoroutine(PlayDialogueChain());
    }

    /// <summary>
    /// Play the initial dialogue chain
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

        isWaitingForAlarm = true;
    }

    /// <summary>
    /// Play a dialogue audio clip
    /// </summary>
    /// <param name="audioClip"></param>
    void PlayDialogue(AudioClip audioClip) {
        audioDialogue.clip = audioClip;
        audioDialogue.Play();
    }

    /// <summary>
    /// Play a dialogue until it finishes
    /// </summary>
    /// <param name="audioClip"></param>
    /// <returns></returns>
    IEnumerator PlayEntireDialogue(AudioClip audioClip) {
        PlayDialogue(audioClip);
        yield return new WaitForSeconds(audioClip.length + dialogueInterval);
    }


    public void SoundTheAlarm() { 
        if (isWaitingForAlarm) {
            isWaitingForAlarm = false;
            StartCoroutine(PlayEntireDialogue(dialogueSoundAlarm));
            StartCoroutine(TimerManager.instance.RunTimer());
            MethodManager.instance.ManageFunctions();
        }
    }
}
