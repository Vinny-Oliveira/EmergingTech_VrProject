using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the pause state of the game
/// Attach this script to the Canvas_Level
/// </summary>
public class PauseManager : MonoBehaviour {

    public DialogueManager dialogueManager;

    private void Start(){
        if (dialogueManager == null) {
            dialogueManager = FindObjectOfType<DialogueManager>();
        }

        ResumeGame(); // Make sure the game is not paused when a level starts
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public void PauseGame() {
        Time.timeScale = 0;
        dialogueManager.audioSource.Pause();
    }

    /// <summary>
    /// Unpause the game
    /// </summary>
    public void ResumeGame() {
        Time.timeScale = 1;
        dialogueManager.audioSource.UnPause();
    }

}
