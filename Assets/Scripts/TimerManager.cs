﻿using System.Collections;
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
    void Start()
    {
        //intTimeRemaining = intTimeTotal;
        StartCoroutine(DisplayTimer());
    }

    //// Update is called once per frame
    //void Update() {
    //    if (intTimer == 0) {
    //        pnl_GameOver.SetActive(true);
    //        return;
    //    }

       
    //    //DisplayTimer(ref intTimer);
    //}

    IEnumerator DisplayTimer() {

        while (intTimer > 0) {

            int intMinutes = intTimer / 60;
            int intSeconds = intTimer % 60;

            string strMinutes = ((intMinutes < 10) ? ("0" + intMinutes) : (intMinutes.ToString()));
            string strSeconds = ((intSeconds < 10) ? ("0" + intSeconds) : (intSeconds.ToString()));
            tmpTimer.text = strMinutes + ":" + strSeconds;

            yield return new WaitForSeconds(1);
            intTimer--;
        }

        pnl_GameOver.SetActive(true);

    }
}
