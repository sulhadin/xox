using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralScript : MonoBehaviour {

    public void LoadScene(string name)
    {
    GameManager.LoadScene(name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
