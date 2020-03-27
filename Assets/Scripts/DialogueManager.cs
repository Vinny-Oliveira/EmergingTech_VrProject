using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueManager : MonoBehaviour {

    public Transform playerCamera;
    public int intWaitTime;
    public const float TURN_AROUND_ANGLE = 120f;

    public AudioSource audioSource;

    public AudioClip dialogueStart;
    public AudioClip dialogueTurnAround;

    private void Start() {
        // Play the dialoque that starts the game
        PlayDialogue(dialogueStart);

        // Check if the player has turned around to face the panel
        StartCoroutine(WaitForRotation());
    }

    /// <summary>
    /// Tell the player to turn around if they have not done so already
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForRotation() {
        float initialRotation = playerCamera.localRotation.eulerAngles.y;
        Debug.Log(initialRotation);
        yield return new WaitForSeconds(intWaitTime);

        while (!(playerCamera.localRotation.eulerAngles.y > initialRotation + TURN_AROUND_ANGLE && 
                 playerCamera.localRotation.eulerAngles.y < initialRotation + 360f - TURN_AROUND_ANGLE)) {
            PlayTurnAround();
            yield return new WaitForSeconds(dialogueTurnAround.length + 1);
            Debug.Log(playerCamera.localRotation.eulerAngles.y);
        }
    }

    /// <summary>
    /// Play dialoque to tell player to turn around
    /// </summary>
    public void PlayTurnAround() {
        PlayDialogue(dialogueTurnAround);
    }

    /// <summary>
    /// Play a dialogue audio clip
    /// </summary>
    /// <param name="audioClip"></param>
    void PlayDialogue(AudioClip audioClip) {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
