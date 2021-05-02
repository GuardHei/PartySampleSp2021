using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneMonobehaviour : MonoBehaviour {

    public string nextScene;
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) LoadScene(nextScene);
    }
}
