using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Valve.VR.InteractionSystem;

/// <summary>
/// Manage how random methids are distributed to the buttons of the game
/// </summary>
public class MethodManager : MonoBehaviour
{
    //public List<VRButton> listButtons = new List<VRButton>();
    public GameObject buttonHolder;
    public GameObject sphereHolder;
    //public GameObject fireHolder;

    //public GameObject ExplodingObj;

    public Transform player;

    // Winning condition sequence
    bool isPanel_Open = false;
    bool isPressureReleased = false;
    bool isDoorOpen = false;

    //[SerializeField]
    //List<Transform> listFire = new List<Transform>();

    // Start is called before the first frame update
    void Start() {
        // Reset winning conditions
        isPanel_Open = false;
        isPressureReleased = false;
        isDoorOpen = false;

        // Reset button list
        List<HoverButton> listButtons = buttonHolder.GetComponentsInChildren<HoverButton>().ToList();

        ////listFire = fireHolder.GetComponentsInChildren<Transform>().ToList();
        //fireHolder.GetComponentsInChildren<Transform>(listFire);
        //listFire.RemoveAt(0); // Remove parent

        // List of methods to be called by the in-game panels
        List<UnityAction<Hand>> listActions = new List<UnityAction<Hand>> {
            DropSpheres,
            ExplodeObject,
            SetFireOff,
            SetFireOff,
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

    #region EVENT_FUNCTIONS

    /// <summary>
    /// Have spheres drop from the ceiling
    /// </summary>
    /// <param name="button"></param>
    void DropSpheres(Hand button) {
        sphereHolder.SetActive(true);
        Debug.Log("Balls");
    }

    ///// <summary>
    ///// Rotate the player away from the panel
    ///// </summary>
    ///// <param name="button"></param>
    //void RotatePlayer(VRButton button) {
    //    float randomNum = UnityEngine.Random.Range(160f, 200f);
    //    player.localRotation *= Quaternion.Euler(0f, randomNum, 0f);
    //    Debug.Log("Rotate");
    //}

    public void ExplodeObject(Hand button) {
        Debug.Log("Explosion");
    }

    public void SetFireOff(Hand button) {
        //listFire.ElementAt(0).gameObject.SetActive(false);
        //listFire.RemoveAt(0);
        Debug.Log("Fire Off");
    }

    void PlayDisasterAnimation(Hand button) {
        // TODO: Play disaster animation on the security panels
        Debug.Log("Disaster played");
    }

    #endregion

    #region BEEPS_AND_BOOPS_FUNCTIONS

    void PlayButtonSound(Hand button) {
        SfxManager.instance.PlayButtonSound();
        Debug.Log("BEEP & BOOP");
    }

    #endregion

    #region WINNING_CONDITION_FUNCTIONS

    void OpenPanel(Hand button) {
        // TODO: Open the panel
        isPanel_Open = true;
        Debug.Log("Panel door is open");
    }

    void ReleasePressure(Hand button) {
        if (!isPanel_Open) {
            PlayButtonSound(button);
            return;
        }

        // TODO: Release the preasure of the pipe
        isPressureReleased = true;
        Debug.Log("Pipe preassure has been released");
    }

    void OpenDoors(Hand button) { 
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
