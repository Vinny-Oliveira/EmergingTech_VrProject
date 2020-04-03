using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Valve.VR;

public class TimerManager : SingletonManager<TimerManager> {

    public int intTimer;
    public TextMeshProUGUI tmpTimer;
    public GameObject pnl_GameOver_VR;
    public GameObject pnl_GameOver_NO_VR;

    /// <summary>
    /// Run the timer
    /// </summary>
    /// <returns></returns>
    public IEnumerator RunTimer() {

        while (intTimer > -1) {

            yield return new WaitForSeconds(1);

            int intMinutes = intTimer / 60;
            int intSeconds = intTimer % 60;

            string strMinutes = ((intMinutes < 10) ? ("0" + intMinutes) : (intMinutes.ToString()));
            string strSeconds = ((intSeconds < 10) ? ("0" + intSeconds) : (intSeconds.ToString()));
            tmpTimer.text = strMinutes + ":" + strSeconds;

            intTimer--;
        }

        if (SteamVR.instance != null) {
            pnl_GameOver_VR.SetActive(true);
        } else {
            pnl_GameOver_NO_VR.SetActive(true);
        }
    }
}
