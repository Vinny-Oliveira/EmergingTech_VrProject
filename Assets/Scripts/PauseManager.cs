using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the pause state of the game
/// Attach this script to the Canvas_Level
/// </summary>
public class PauseManager : MonoBehaviour {

    /// <summary>
    /// Pause the game
    /// </summary>
    public void PauseGame() {
        Time.timeScale = 0;
        DialogueManager.instance.PauseDialogues();
        SfxManager.instance.PauseEverySfx();
    }

    /// <summary>
    /// Unpause the game
    /// </summary>
    public void ResumeGame() {
        Time.timeScale = 1;
        DialogueManager.instance.UnPauseDialogues();
        SfxManager.instance.UnPauseEverySfx();
    }

}
