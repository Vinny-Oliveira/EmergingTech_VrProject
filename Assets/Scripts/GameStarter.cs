using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour {

    public void StartGame() {
        DialogueManager.instance.StartDialogueChain();
        Time.timeScale = 1f;
    }

}