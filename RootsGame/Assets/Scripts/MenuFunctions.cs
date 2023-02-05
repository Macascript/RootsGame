using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadSceneAsync("CarvasScene");
    }

    public void Menu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }
}
