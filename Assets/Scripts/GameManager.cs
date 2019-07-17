using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static Levels Level { get; set; }

    public enum Levels
    {
        Easy,
        Normal,
        Hard
    }

    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}