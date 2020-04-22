using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Valve.VR;

public class TimerManager : SingletonManager<TimerManager> {

    public int intTimer;
    public TextMeshProUGUI tmpTimer;
    public GameObject gamePanel;
    public GameObject restartPanel;
    public LightChanger lightChanger;
    public SceneField gameOverScene;

    /// <summary>
    /// Run the timer
    /// </summary>
    /// <returns></returns>
    public IEnumerator RunTimer() {

        // Countdown
        while (intTimer > -1 && !MethodManager.instance.IsDoorOpen()) {

            yield return new WaitForSeconds(1);

            int intMinutes = intTimer / 60;
            int intSeconds = intTimer % 60;

            string strMinutes = ((intMinutes < 10) ? ("0" + intMinutes) : (intMinutes.ToString()));
            string strSeconds = ((intSeconds < 10) ? ("0" + intSeconds) : (intSeconds.ToString()));
            tmpTimer.text = strMinutes + ":" + strSeconds;

            intTimer--;
        }

        // Activate the game over panel
        if (!MethodManager.instance.IsDoorOpen()) {
            PlayLossEvent();
        }
    }

    /// <summary>
    /// Play the event for game loss
    /// </summary>
    public void PlayLossEvent() {
        DialogueManager.instance.PlayEndGameDialogue(false);
        gamePanel.SetActive(false);
        restartPanel.SetActive(true);
        lightChanger.StartLightIncrease();
    }

    /// <summary>
    /// Load the game over scene
    /// </summary>
    public void LoadGameOverScene() {
        SceneManager.LoadScene(gameOverScene);
    }
}
