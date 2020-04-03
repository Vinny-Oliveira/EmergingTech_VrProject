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
    bool isWaitingForAlarm = false;
    bool isTimerRunning = false;

    private void Start() {
        isWaitingForAlarm = false;
        isTimerRunning = false;

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

        //// Play the dialoque that starts the game
        //yield return StartCoroutine(PlayEntireDialogue(dialogueStart));

        //// Check if player has turned around to face the button panel
        //while (!(playerCamera.localRotation.eulerAngles.y > initialRotation + TURN_AROUND_ANGLE && 
        //         playerCamera.localRotation.eulerAngles.y < initialRotation + 360f - TURN_AROUND_ANGLE)) {
        //    yield return StartCoroutine(PlayEntireDialogue(dialogueTurnAround));
        //}

        //// Play the following dialogue chain after the player has turned around
        //yield return StartCoroutine(PlayEntireDialogue(dialogueFacePanel));
        //yield return StartCoroutine(PlayEntireDialogue(dialogueBackToWork));
        //yield return StartCoroutine(PlayEntireDialogue(dialogueSayHoldOn));
        //yield return StartCoroutine(PlayEntireDialogue(dialogueCantSpeak));

        // Sound the alarm if the player presses a button or if nothing is pressed for a while
        isWaitingForAlarm = true;
        yield return new WaitForSeconds(waitTimeForAlarm);
        if (!isTimerRunning) {
            SoundTheAlarm();
        }
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

    /// <summary>
    /// Sound the alarm, start the timer, and enable the buttons
    /// </summary>
    public void SoundTheAlarm() { 
        if (isWaitingForAlarm) {
            isWaitingForAlarm = false;
            isTimerRunning = true;
            StartCoroutine(PlayEntireDialogue(dialogueSoundAlarm));
            StartCoroutine(TimerManager.instance.RunTimer());
            MethodManager.instance.ManageFunctions();
        }
    }
}
