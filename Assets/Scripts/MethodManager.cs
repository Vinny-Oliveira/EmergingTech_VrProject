using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Valve.VR;
using Valve.VR.InteractionSystem;

/// <summary>
/// Manage how random methods are distributed to the buttons of the game
/// </summary>
public class MethodManager : SingletonManager<MethodManager> {

    [Header("Event and button holders")]
    public GameObject buttonHolder;
    public GameObject sphereHolder;
    public GameObject videoCanvas;
    public GameObject confetti;

    [Header("References for progression")]
    public Animator animPanelDoor;
    public GameObject brokenPipe;
    public GameObject fixedPipe;
    public GameObject pauseBtn_VR;
    public GameObject pauseBtn_NO_VR;
    public GameObject pnl_Win_VR;
    public GameObject pnl_Win_NO_VR;

    // Winning condition sequence
    bool isPanel_Open = false;
    bool isPressureReleased = false;
    bool isDoorOpen = false;

    /// <summary>
    /// Manage how functions are placed in the buttons of the level
    /// </summary>
    public void ManageFunctions() {
        // Reset winning conditions
        isPanel_Open = false;
        isPressureReleased = false;
        isDoorOpen = false;


        // Reset button list
        List<HoverButton> listButtons = buttonHolder.GetComponentsInChildren<HoverButton>().ToList();

        // List of methods to be called by the in-game panels
        List<UnityAction<Hand>> listActions = new List<UnityAction<Hand>> {
            DropSpheres,
            ExplodeObject,
            DropConfetti,
            PlayButtonSound,
            PlayDisasterAnimation,
            PlayButtonSound,
            OpenPanel,
            ReleasePressure,
            OpenDoors
        };

        // Randomize the methods
        ListRandomizer.Randomize(ref listActions);

        // Populate the buttons with the methods
        PopulateButtons(ref listButtons, ref listActions);
    }

    /// <summary>
    /// Populate the buttons of a list with methods of a second list
    /// </summary>
    /// <param name="vrButtons"></param>
    /// <param name="unityActions"></param>
    void PopulateButtons(ref List<HoverButton> vrButtons, ref List<UnityAction<Hand>> unityActions) { 
        for (int i = 0; i < vrButtons.Count; i++) {
            HoverButton button = vrButtons[i];
            int index = i % unityActions.Count;
            button.onButtonIsPressed.AddListener(unityActions[index]);
        }
    }

    /// <summary>
    /// Getter for isDoorOpen
    /// </summary>
    /// <returns></returns>
    public bool IsDoorOpen() {
        return isDoorOpen;
    }

    #region EVENT_FUNCTIONS

    /// <summary>
    /// Have spheres drop from the ceiling
    /// </summary>
    /// <param name="button"></param>
    void DropSpheres(Hand button) {
        ActivateEvent(sphereHolder, button);
        Debug.Log("Balls");
    }

    /// <summary>
    /// Drop confetti from the ceiling
    /// </summary>
    /// <param name="button"></param>
    void DropConfetti(Hand button) {
        ActivateEvent(confetti, button);
        Debug.Log("Confetti");
    }

    /// <summary>
    /// Play a disaster video on the screen
    /// </summary>
    /// <param name="button"></param>
    void PlayDisasterAnimation(Hand button) {
        ActivateEvent(videoCanvas, button);
        Debug.Log("Disaster played");
    }

    void ExplodeObject(Hand button) {
        PlayButtonSound(button);
        SfxManager.instance.PlaySfx(SfxManager.instance.audioExplosion);
        Debug.Log("Explosion");
    }

    /// <summary>
    /// Play a button sound and then activate a game object if it is not active
    /// </summary>
    /// <param name="eventObject"></param>
    /// <param name="button"></param>
    void ActivateEvent(GameObject eventObject, Hand button) {
        PlayButtonSound(button);
        if (!eventObject.activeInHierarchy){
            eventObject.SetActive(true);
        }
    }

    #endregion

    #region BEEPS_AND_BOOPS_FUNCTIONS

    protected void PlayButtonSound(Hand button) {
        SfxManager.instance.PlayButtonSound();
        Debug.Log("BEEP & BOOP");
    }

    #endregion

    #region WINNING_CONDITION_FUNCTIONS

    /// <summary>
    /// Open the door of the panel that has a broken pipe
    /// </summary>
    /// <param name="button"></param>
    void OpenPanel(Hand button) {
        PlayButtonSound(button);

        // Do nothing if the panel is already open
        if (isPanel_Open) {
            PlayButtonSound(button);
            return;
        }

        // Play the animation of the panel openning
        animPanelDoor.SetTrigger(animPanelDoor.parameters[0].name);

        isPanel_Open = true;
        Debug.Log("Panel door is open");
    }

    /// <summary>
    /// Fix the broken pipe inside the panel
    /// </summary>
    /// <param name="button"></param>
    void ReleasePressure(Hand button) {
        PlayButtonSound(button);

        // Do nothing if the panel is closed or if the preassure has already been released
        if (!isPanel_Open || isPressureReleased) {
            PlayButtonSound(button);
            return;
        }

        // Release the preasure of the pipe
        fixedPipe.SetActive(true);
        brokenPipe.SetActive(false);

        isPressureReleased = true;
        Debug.Log("Pipe preassure has been released");
    }

    /// <summary>
    /// Open the door in the back of the room and win the level
    /// </summary>
    /// <param name="button"></param>
    void OpenDoors(Hand button) {
        PlayButtonSound(button);

        if (!isPanel_Open || !isPressureReleased || isDoorOpen) {
            PlayButtonSound(button);
            return;
        }

        isDoorOpen = true;
        StartCoroutine(PlayWinEvents());

        Debug.Log("Exit doors are open");
    }

    IEnumerator PlayWinEvents() { 
        SfxManager.instance.alarmAudioSource.Stop();

        yield return new WaitForSeconds(3);

        if (SteamVR.instance != null) {
            pauseBtn_VR.SetActive(false);
            pnl_Win_VR.SetActive(true);
        } else {
            pauseBtn_NO_VR.SetActive(false);
            pnl_Win_NO_VR.SetActive(true);
        }
    }

    #endregion
}
