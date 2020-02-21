using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodManager : MonoBehaviour
{
    delegate void MyMethod();
    public List<VRButton> listButtons = new List<VRButton>();
    
    // Start is called before the first frame update
    void Start() {
        List<MyMethod> listMethods = new List<MyMethod>();

        listMethods.Add(DoFirstThing);
        listMethods.Add(DoSecondThing);
        listMethods.Add(DoThirdThing);

        ListRandomizer.Randomize(listMethods);

        for (int i = 0; i < listButtons.Count; i++) {
            VRButton button = listButtons[i];
            button.ButtonListeners.AddListener(DoFirstThing);
        }
    }

    private void DoFirstThing(VRButton arg0)
    {
        throw new NotImplementedException();
    }

    void DoFirstThing() {
        Debug.Log("1st thing done");
    }

    void DoSecondThing() {
        Debug.Log("2nd thing done");
    }

    void DoThirdThing() {
        Debug.Log("3rd thing done");
    }
}
