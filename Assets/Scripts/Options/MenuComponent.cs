using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuComponent : MonoBehaviour {

    public Canvas mainMenuRef;
    public Canvas optionsRef;

    void Awake()
    {
        ShowMenu();
    }

    public void StartLevel()
    {
        Application.LoadLevel("Inventory");
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
