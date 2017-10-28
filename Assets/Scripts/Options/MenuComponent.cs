using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuComponent : MonoBehaviour {

    public int levelIndex;
    public Canvas mainMenuRef;
    public Canvas optionsRef;

    void Awake()
    {
        ShowMenu();
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(levelIndex, LoadSceneMode.Single);
    }

    public void ShowMenu()
    {
        mainMenuRef.enabled = true;
        optionsRef.enabled = false;
    }

    public void ShowOptions()
    {
        mainMenuRef.enabled = false;
        optionsRef.enabled = true;
    }
}
