using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {


    public void Easy()
    {
        GameManager.Level = GameManager.Levels.Easy;
        GameManager.LoadScene("main");
    }
    public void Normal()
    {
        GameManager.Level = GameManager.Levels.Normal;
        GameManager.LoadScene("main");

    }
    public void Hard()
    {
        GameManager.Level = GameManager.Levels.Hard;
        GameManager.LoadScene("main");

    }
}