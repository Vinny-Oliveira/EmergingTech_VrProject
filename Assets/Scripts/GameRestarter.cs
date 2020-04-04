using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour {

    public void RestartGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().name.ToString());
    }

}