using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScreenManager : MonoBehaviour {

    [System.Serializable]
    public class UIScreenMapping {
        public string InputButton;
        public GameObject ScreenObject;
        [System.NonSerialized]
        private IUIScreen cachedScreen;
        public IUIScreen Screen {
            get {
                if (cachedScreen != null)
                    return cachedScreen;
                return cachedScreen = ScreenObject?.GetComponent<IUIScreen>();
            }
        }
    }

    public UIScreenMapping[] Screens = new UIScreenMapping[0];

    public UIScreenMapping CurrentScreen { get; private set; }

    void Start() {
        foreach (UIScreenMapping screen in Screens)
            screen.Screen.IsVisible = false;
    }

    void Update() {
        UIScreenMapping prevScreen = CurrentScreen;

        foreach (UIScreenMapping screen in Screens) {
            if (Input.GetButtonDown(screen.InputButton)) {
                if (CurrentScreen == screen)
                    CurrentScreen = null;
                else
                    CurrentScreen = screen;
            }
        }

        if (CurrentScreen != prevScreen)
            foreach (UIScreenMapping screen in Screens)
                screen.Screen.IsVisible = screen == CurrentScreen;
    }

}
