using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public int intTimer;
    //public int intTimeTotal;
    public TextMeshProUGUI tmpTimer;
    public GameObject pnl_GameOver;

    //// Start is called before the first frame update
    //void Start() {
    //    intTimeRemaining = intTimeTotal;
    //}

    // Update is called once per frame
    void Update() {
        if (intTimer == 0) {
            pnl_GameOver.SetActive(true);
            return;
        }

        StartCoroutine(CountOneSecond());
    }

    IEnumerator CountOneSecond() {
        int minutes = intTimer / 60;
        int seconds = intTimer % 60;
        tmpTimer.text = minutes + ":" + seconds;
        
        yield return new WaitForSeconds(1);
    }
}
