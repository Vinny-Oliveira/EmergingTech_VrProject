using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MethodManager : MonoBehaviour
{
    public List<VRButton> listButtons = new List<VRButton>();

    // Start is called before the first frame update
    void Start() {
        // List of methods to be called by the in-game pannels
        List<UnityAction<VRButton>> listActions = new List<UnityAction<VRButton>> {
            DoFirstThing,
            DoSecondThing,
            DoThirdThing
        };

        // Randomize the methods
        ListRandomizer.Randomize(listActions);

        // Populate the buttons with the methods
        PopulateButtons(listButtons, listActions);
    }

    /// <summary>
    /// Populate the buttons of a list with methods of a second list
    /// </summary>
    /// <param name="unityActions"></param>
    void PopulateButtons(List<VRButton> vrButtons, List<UnityAction<VRButton>> unityActions) { 
        for (int i = 0; i < vrButtons.Count; i++) {
            VRButton button = vrButtons[i];
            int index = i % unityActions.Count;
            button.ButtonListeners.AddListener(unityActions[index]);
        }
    }

    public void DoFirstThing(VRButton button) {
        Debug.Log("1st thing done");
    }

    void DoSecondThing(VRButton button) {
        Debug.Log("2nd thing done");
    }

    void DoThirdThing(VRButton button) {
        Debug.Log("3rd thing done");
    }
}
