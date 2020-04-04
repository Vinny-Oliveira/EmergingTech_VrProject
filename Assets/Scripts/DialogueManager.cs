using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRInteractions;

[RequireComponent(typeof(AudioSource))]
public class DialogueManager : SingletonManager<DialogueManager> {

    [Header("Control Variables")]
    public Transform playerCamera;
    public int dialogueInterval;
    public int waitTimeForAlarm;
    public int waitTimeForDoor;
    public const float TURN_AROUND_ANGLE = 120f;

    [Header("Audio Source and Clips")]
    public AudioSource audioDialogue;
    public AudioClip dialogueStart;
    public AudioClip dialogueTurnAround;
    public AudioClip dialogueFacePanel;
    public AudioClip dialogueBackToWork;
    public AudioClip dialogueSayHoldOn;
    public AudioClip dialogueCantSpeak;
    public AudioClip dialogueSoundAlarmByButton;
    public AudioClip dialogueSoundAlarmByIdle;
    public AudioClip dialogueDoorKnock1;
    public AudioClip dialogueDoorKnock2;
    public AudioClip dialoguePressDoorButton;

    // Dialogue control
    bool isWaitingForAlarm = false;
    bool isWaitingForDoor = false;
    bool isSkipped = false;

    private void Update() {
        // Press escape to skip dialogue
        if (Input.GetKeyDown(KeyCode.Escape)) {
            audioDialogue.Stop();
            isSkipped = true;
        }
    }

    public void StartDialogueChain() {
        isSkipped = false;
        isWaitingForAlarm = false;
        isWaitingForDoor = false;

        // Play the initial dialogue chain
        StartCoroutine(PlayDialogueChain());
        Debug.Log("Press Esc to skip dialogue");
    }

    /// <summary>
    /// Play the initial dialogue chain
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayDialogueChain() {
        // Set initial rotation
        float initialRotation = playerCamera.localRotation.eulerAngles.y;
        yield return new WaitForSeconds(dialogueInterval);

        // Play the dialoque that starts the game
        if (!isSkipped) yield return StartCoroutine(PlayEntireDialogue(dialogueStart));

        // Check if player has turned around to face the button panel
        while (!(playerCamera.localRotation.eulerAngles.y > initialRotation + TURN_AROUND_ANGLE && 
                 playerCamera.localRotation.eulerAngles.y < initialRotation + 360f - TURN_AROUND_ANGLE) && 
               !isSkipped) {
            yield return StartCoroutine(PlayEntireDialogue(dialogueTurnAround));
        }

        // Play the following dialogue chain after the player has turned around
        if (!isSkipped) yield return StartCoroutine(PlayEntireDialogue(dialogueFacePanel));
        if (!isSkipped) yield return StartCoroutine(PlayEntireDialogue(dialogueBackToWork));
        if (!isSkipped) yield return StartCoroutine(PlayEntireDialogue(dialogueSayHoldOn));
        if (!isSkipped) yield return StartCoroutine(PlayEntireDialogue(dialogueCantSpeak));

        // Sound the alarm if the player presses a button or if nothing is pressed for a while
        isWaitingForAlarm = true;
        yield return new WaitForSeconds(waitTimeForAlarm);
        if (isWaitingForAlarm) {
            SoundTheAlarm(dialogueSoundAlarmByIdle);
        }
        yield return new WaitWhile(() => audioDialogue.isPlaying);

        // Have engineers come to the door
        isWaitingForDoor = true;
        Debug.Log("Waiting for door");
        yield return new WaitForSeconds(waitTimeForDoor);
        if (isWaitingForDoor) {
            EngineerKnocksOnDoor_1();
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
    /// Play a dialogue until it finishes or is skipped
    /// </summary>
    /// <param name="audioClip"></param>
    /// <returns></returns>
    IEnumerator PlayEntireDialogue(AudioClip audioClip) {
        PlayDialogue(audioClip);
        yield return new WaitWhile(() => audioDialogue.isPlaying);
        if (!isSkipped) { 
            yield return new WaitForSeconds(dialogueInterval);
        } 
    }

    /// <summary>
    /// Play a dialogue if a condition is satisfied and then lock that condition
    /// </summary>
    /// <param name="audioToPlay"></param>
    /// <param name="waitCondition"></param>
    bool PlayDialogueOnConditions(AudioClip audioToPlay, ref bool waitCondition) { 
        if (waitCondition) {
            waitCondition = false;
            StartCoroutine(PlayEntireDialogue(audioToPlay));
            return true;
        }

        return false;
    }

    //IEnumerator PlayDialogueAfterWaiting(AudioClip audioToPlay, int waitTime, System.Action<bool> waitCondition) { 
    //    waitCondition(true);
    //    yield return new WaitForSeconds(waitTime);
    //    if (waitCondition) {
    //        SoundTheAlarm(audioToPlay);
    //    }
    //}

    /// <summary>
    /// Sound the alarm, start the timer, and enable the buttons
    /// </summary>
    public void SoundTheAlarm() {
        SoundTheAlarm(dialogueSoundAlarmByButton);
    }

    /// <summary>
    /// Sound the alarm, start the timer, and enable the buttons
    /// </summary>
    public void SoundTheAlarm(AudioClip audioClip) { 
        if (PlayDialogueOnConditions(audioClip, ref isWaitingForAlarm)) { 
            StartCoroutine(TimerManager.instance.RunTimer());
            MethodManager.instance.ManageFunctions();
            MethodManager.instance.brokenPipe.SetActive(true);
        }
    }

    /// <summary>
    /// Play the first dialogue of the engineer knocking on the door
    /// </summary>
    public void EngineerKnocksOnDoor_1() {
        PlayDialogueOnConditions(dialogueDoorKnock1, ref isWaitingForDoor);
    }
}
