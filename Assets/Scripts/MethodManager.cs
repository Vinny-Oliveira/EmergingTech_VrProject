using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Valve.VR.InteractionSystem;
//using MethodUtil;

/// <summary>
/// Manage how random methods are distributed to the buttons of the game
/// </summary>
public class MethodManager : MethodUtil {

    // Start is called before the first frame update
    void Start() {
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
}
