using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public int intTimeRemaining;
    public int intTimeTotal;
    public Text txtTimeRemaining;

    // Start is called before the first frame update
    void Start() {
        intTimeRemaining = intTimeTotal;
    }

    // Update is called once per frame
    void Update() {
        if (intTimeRemaining == 0) {
            return;
        }

        StartCoroutine(CountOneSecond());
    }

    IEnumerator CountOneSecond() {
        yield return new WaitForSeconds(1);
        int minutes = intTimeRemaining / 60;
        int seconds = intTimeRemaining % 60;

        txtTimeRemaining.text = minutes + ":" + seconds;
    }
}
