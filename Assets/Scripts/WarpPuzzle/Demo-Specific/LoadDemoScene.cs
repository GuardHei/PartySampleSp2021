using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDemoScene: MonoBehaviour
{
    public int sceneNumber;
    public void Invoke()
    {
        SceneManager.LoadScene("DemoScene" + sceneNumber);
    }
}
