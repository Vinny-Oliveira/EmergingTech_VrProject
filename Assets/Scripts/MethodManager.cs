using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodManager : MonoBehaviour
{
    public delegate void MyMethod(VRButton button);
    public List<VRButton> listButtons = new List<VRButton>();
    
    // Start is called before the first frame update
    void Start() {
        List<MyMethod> listMethods = new List<MyMethod> {
            DoFirstThing,
            DoSecondThing,
            DoThirdThing
        };
        
        ListRandomizer.Randomize(listMethods);

        for (int i = 0; i < listButtons.Count; i++) {
            VRButton button = listButtons[i];
            //button.ButtonListeners.AddListener(listMethods[i]);
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
