using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Valve.VR.InteractionSystem;

public class MethodUtil : MonoBehaviour {

    [Header("Event and button holders")]
    public GameObject buttonHolder;
    public GameObject sphereHolder;

    [Header("References for progressions")]
    public Animator animPanelDoor;
    public GameObject brokenPipe;
    public GameObject fixedPipe;

    // Winning condition sequence
    protected bool isPanel_Open = false;
    protected bool isPressureReleased = false;
    protected bool isDoorOpen = false;

    #region EVENT_FUNCTIONS

    /// <summary>
    /// Have spheres drop from the ceiling
    /// </summary>
    /// <param name="button"></param>
    protected void DropSpheres(Hand button) {
        PlayButtonSound(button);
        if (sphereHolder.activeInHierarchy) {
            return;
        }
        
        sphereHolder.SetActive(true);
        Debug.Log("Balls");
    }

    protected void ExplodeObject(Hand button) {
        PlayButtonSound(button);
        Debug.Log("Explosion");
    }

    protected void SetFireOff(Hand button) {
        PlayButtonSound(button);
        Debug.Log("Fire Off");
    }

    protected void PlayDisasterAnimation(Hand button) {
        PlayButtonSound(button);

        // TODO: Play disaster animation on the security panels
        Debug.Log("Disaster played");
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
    protected void OpenPanel(Hand button) {
        PlayButtonSound(button);

        // Do nothing if the panel is already open
        if (isPanel_Open) {
            PlayButtonSound(button);
            return;
        }

        // Play the animation of the panel openning
        animPanelDoor.SetTrigger("Open");

        isPanel_Open = true;
        Debug.Log("Panel door is open");
    }

    /// <summary>
    /// Fix the broken pipe inside the panel
    /// </summary>
    /// <param name="button"></param>
    protected void ReleasePressure(Hand button) {
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

    protected void OpenDoors(Hand button) {
        PlayButtonSound(button);

        if (!isPanel_Open || !isPressureReleased) {
            PlayButtonSound(button);
            return;
        }

        // TODO: Open the doors and win the game
        isDoorOpen = true;
        Debug.Log("Exit doors are open");
    }

    #endregion

}
