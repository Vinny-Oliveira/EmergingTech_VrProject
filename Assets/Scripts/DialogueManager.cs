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
    public int waitTimeForButton;
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
    bool isGamePaused = false;
    bool isSkipped = false;
    bool isWaitingForAlarm = false;
    bool isWaitingForDoor = false;
    bool isWaitingForBtnPress = false;

    private void Update() {
        // Press escape to skip dialogue
        if (Input.GetKeyDown(KeyCode.Escape)) {
            audioDialogue.Stop();
            isSkipped = true;
        }
    }

    /// <summary>
    /// Start the dialogue chain of the beginning of the game
    /// </summary>
    public void StartDialogueChain() {
        // Initialize control variables
        isGamePaused = false;
        isSkipped = false;
        isWaitingForAlarm = false;
        isWaitingForDoor = false;
        isWaitingForBtnPress = false;

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
        //float initialRotation = playerCamera.localRotation.eulerAngles.y;
        yield return new WaitForSeconds(dialogueInterval);

        // Play the dialoque that starts the game
        if (!isSkipped) yield return StartCoroutine(PlayEntireDialogue(dialogueStart));

        //// Check if player has turned around to face the button panel
        //while (!(playerCamera.localRotation.eulerAngles.y > initialRotation + TURN_AROUND_ANGLE && 
        //         playerCamera.localRotation.eulerAngles.y < initialRotation + 360f - TURN_AROUND_ANGLE) && 
        //       !isSkipped) {
        //    yield return StartCoroutine(PlayEntireDialogue(dialogueTurnAround));
        //}

        // Play the following dialogue chain after the player has turned around
        if (!isSkipped) yield return StartCoroutine(PlayEntireDialogue(dialogueFacePanel));
        if (!isSkipped) yield return StartCoroutine(PlayEntireDialogue(dialogueBackToWork));
        if (!isSkipped) yield return StartCoroutine(PlayEntireDialogue(dialogueSayHoldOn));
        if (!isSkipped) yield return StartCoroutine(PlayEntireDialogue(dialogueCantSpeak));

        // Sound the alarm if the player presses a button or if nothing is pressed for a while
        yield return StartCoroutine(PlayDialogueAfterWaiting(waitTimeForAlarm, SoundTheAlarm, (myBool) => { isWaitingForAlarm = myBool; }, () => isWaitingForAlarm));
        yield return new WaitUntil(() => (!audioDialogue.isPlaying && !isGamePaused));

        // Have engineers come to the door if a button is pushed or if nothing is done for a while
        yield return StartCoroutine(PlayDialogueAfterWaiting(waitTimeForDoor, EngineerKnocksOnDoor_1, (myBool) => { isWaitingForDoor = myBool; }, () => isWaitingForDoor));
        yield return new WaitUntil(() => (!audioDialogue.isPlaying && !isGamePaused));

        // Engineers cannot open the door - this dialogue is skipped if the door button is pushed
        yield return StartCoroutine(PlayDialogueAfterWaiting(waitTimeForButton, EngineerKnocksOnDoor_2, (myBool) => { isWaitingForBtnPress = myBool; }, () => isWaitingForBtnPress));
        yield return new WaitUntil(() => (!audioDialogue.isPlaying && !isGamePaused));
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
        yield return new WaitUntil(() => (!audioDialogue.isPlaying && !isGamePaused));
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

    /// <summary>
    /// Play a dialogue after waiting for some time if a caondition is satisfied
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="SoundAction"></param>
    /// <param name="WaitCondition"></param>
    /// <param name="ConditionChecker"></param>
    /// <returns></returns>
    IEnumerator PlayDialogueAfterWaiting(int waitTime, System.Action SoundAction, System.Action<bool> WaitCondition, System.Func<bool> ConditionChecker) { 
        WaitCondition(true);
        yield return new WaitForSeconds(waitTime);
        if (ConditionChecker()) {
            SoundAction();
        }
    }

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
            SfxManager sfxManager = SfxManager.instance;
            sfxManager.alarmAudioSource.Play();
            StartCoroutine(TimerManager.instance.RunTimer());
            MethodManager.instance.ManageFunctions();
            MethodManager.instance.brokenPipe.SetActive(true);
            sfxManager.pipeLeakAudioSource.Play();
            LightChanger.instance.StartDimmer();
        }
    }

    /// <summary>
    /// Play the first dialogue of the engineer knocking on the door
    /// </summary>
    public void EngineerKnocksOnDoor_1() {
        EngineerKnocksOnDoor_1(dialogueDoorKnock1);
    }
    
    /// <summary>
    /// Play the first dialogue of the engineer knocking on the door
    /// </summary>
    public void EngineerKnocksOnDoor_1(AudioClip audioClip) {
        PlayDialogueOnConditions(audioClip, ref isWaitingForDoor);
    }
        
    /// <summary>
    /// Play the first dialogue of the engineer knocking on the door
    /// </summary>
    public void EngineerKnocksOnDoor_2() {
        EngineerKnocksOnDoor_2(dialogueDoorKnock2);
    }
        
    /// <summary>
    /// Play the first dialogue of the engineer knocking on the door
    /// </summary>
    public void EngineerKnocksOnDoor_2(AudioClip audioClip) {
        PlayDialogueOnConditions(audioClip, ref isWaitingForBtnPress);
    }

    /// <summary>
    /// Set the pause state of the game to work with dialogues
    /// </summary>
    /// <param name="pauseState"></param>
    public void SetGamePaused(bool pauseState) {
        isGamePaused = pauseState;
    }
}
