using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuComponent : MonoBehaviour {

    public int levelIndex;
    public Canvas mainMenuRef;
    public Canvas optionsRef;
    public AudioManager audioManager;

    void Awake()
    {
        audioManager = AudioManager.Instance;
        ShowMenu();
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(levelIndex, LoadSceneMode.Single);
        audioManager.PlayDungeonSound();
    }

    public void ShowMenu()
    {
        mainMenuRef.enabled = true;
        optionsRef.enabled = false;
        audioManager.PlayMenuSound();
    }

    public void ShowOptions()
    {
        mainMenuRef.enabled = false;
        optionsRef.enabled = true;
    }
}
