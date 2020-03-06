using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class MethodManager : MonoBehaviour
{
    //public List<VRButton> listButtons = new List<VRButton>();
    public GameObject buttonHolder;
    public GameObject sphereHolder;
    public GameObject fireHolder;

    public GameObject ExplodingObj;

    public Transform player;

    [SerializeField]
    List<Transform> listFire = new List<Transform>();

    // Start is called before the first frame update
    void Start() {
        List<VRButton> listButtons = buttonHolder.GetComponentsInChildren<VRButton>().ToList();

        //listFire = fireHolder.GetComponentsInChildren<Transform>().ToList();
        fireHolder.GetComponentsInChildren<Transform>(listFire);
        listFire.RemoveAt(0);

        // List of methods to be called by the in-game pannels
        List<UnityAction<VRButton>> listActions = new List<UnityAction<VRButton>> {
            DropSpheres,
            RotatePlayer,
            ExplodeObject,
            SetFireOff
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
    void PopulateButtons(ref List<VRButton> vrButtons, ref List<UnityAction<VRButton>> unityActions) { 
        for (int i = 0; i < vrButtons.Count; i++) {
            VRButton button = vrButtons[i];
            int index = i % unityActions.Count;
            button.ButtonListeners.AddListener(unityActions[index]);
        }
    }

    /// <summary>
    /// Have spheres drop from the ceiling
    /// </summary>
    /// <param name="button"></param>
    void DropSpheres(VRButton button) {
        sphereHolder.SetActive(true);
    }

    /// <summary>
    /// Rotate the player away from the panel
    /// </summary>
    /// <param name="button"></param>
    void RotatePlayer(VRButton button) {
        float randomNum = UnityEngine.Random.Range(160f, 200f);
        player.localRotation *= Quaternion.Euler(0f, randomNum, 0f);
    }

    public void ExplodeObject(VRButton button) {
        Debug.Log("Explosion");
    }

    public void SetFireOff(VRButton button) {
        listFire.ElementAt(0).gameObject.SetActive(false);
        listFire.RemoveAt(0);
    }

    //[ContextMenu("FIRE OFF")]
    //public void SetFireOff() {
    //    listFire.ElementAt(0).gameObject.SetActive(false);
    //    listFire.RemoveAt(0);
    //}
}
