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
        public AudioClip ClipOnClose;
        public AudioClip ClipOnOpen;
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

    public AudioSource Audio;

    void Awake() {
        if (Audio == null)
            Audio = GetComponent<AudioSource>();
    }

    void Start() {
        foreach (UIScreenMapping screen in Screens)
            screen.Screen.IsVisible = false;
    }

    void Update() {
        UIScreenMapping prevScreen = CurrentScreen;

        foreach (UIScreenMapping screen in Screens) {
            if (Input.GetButtonDown(screen.InputButton)) {
                if (CurrentScreen == screen) {
                    if (screen.ClipOnClose != null)
                        Audio.PlayOneShot(screen.ClipOnClose);
                    CurrentScreen = null;
                    Time.timeScale = 1f;
                } else {
                    if (screen.ClipOnOpen != null)
                        Audio.PlayOneShot(screen.ClipOnOpen);
                    CurrentScreen = screen;
                    Time.timeScale = 0f;
                }
            }
        }

        if (CurrentScreen != prevScreen)
            foreach (UIScreenMapping screen in Screens)
                screen.Screen.IsVisible = screen == CurrentScreen;
    }

}
