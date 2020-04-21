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
    public AudioClip dialogueWin;
    public AudioClip dialogueLoss;

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
        yield return new WaitForSeconds(dialogueInterval);

        // Play the dialoque that starts the game
        if (!isSkipped) yield return StartCoroutine(PlayEntireDialogue(dialogueStart));
        if (!isSkipped) yield return StartCoroutine(PlayEntireDialogue(dialogueFacePanel));
        if (!isSkipped) yield return StartCoroutine(PlayEntireDialogue(dialogueBackToWork));
        if (!isSkipped) yield return StartCoroutine(PlayEntireDialogue(dialogueSayHoldOn));
        if (!isSkipped) yield return StartCoroutine(PlayEntireDialogue(dialogueCantSpeak));

        // Sound the alarm if the player presses a button or if nothing is pressed for a while
        yield return StartCoroutine(PlayDialogueAfterWaiting(waitTimeForAlarm, SoundTheAlarm, (myBool) => { isWaitingForAlarm = myBool; }, () => isWaitingForAlarm));

        // Have engineers come to the door if a button is pushed or if nothing is done for a while
        yield return StartCoroutine(PlayDialogueAfterWaiting(waitTimeForDoor, EngineerKnocksOnDoor_1, (myBool) => { isWaitingForDoor = myBool; }, () => isWaitingForDoor));

        // Engineers cannot open the door - this dialogue is skipped if the door button is pushed
        yield return StartCoroutine(PlayDialogueAfterWaiting(waitTimeForButton, EngineerKnocksOnDoor_2, (myBool) => { isWaitingForBtnPress = myBool; }, () => isWaitingForBtnPress));
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
        yield return new WaitUntil(() => (!audioDialogue.isPlaying && !isGamePaused));
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
            sfxManager.pipeLeakAudioSource.Play();

            StartCoroutine(TimerManager.instance.RunTimer());

            MethodManager methodManager = MethodManager.instance;
            methodManager.ManageFunctions();
            methodManager.brokenPipe.SetActive(true);
            RandomizeMaterials[] buttons = methodManager.buttonHolder.GetComponentsInChildren<RandomizeMaterials>();
            foreach (var button in buttons) {
                button.StartRandomizer();
            }
            
            LightChanger.instance.StartDimmer();
        }
    }

    /// <summary>
    /// Play dialogues for win and loss conditions
    /// </summary>
    /// <param name="isGameWon"></param>
    public void PlayEndGameDialogue(bool isGameWon) {
        if (isGameWon) {
            StartCoroutine(PlayEntireDialogue(dialogueWin));
        } else {
            StartCoroutine(PlayEntireDialogue(dialogueLoss));
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
    /// Pause dialogues
    /// </summary>
    public void PauseDialogues() {
        isGamePaused = true;
        audioDialogue.Pause();
    }

    /// <summary>
    /// Unpause dialogues
    /// </summary>
    public void UnPauseDialogues() {
        isGamePaused = false;
        audioDialogue.UnPause();
    }
}
