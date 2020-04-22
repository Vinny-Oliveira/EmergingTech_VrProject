using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour {

    public SceneField sceneToLoad;

    public void RestartGame() {
        SceneManager.LoadScene(sceneToLoad);
    }

}