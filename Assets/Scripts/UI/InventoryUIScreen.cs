using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIScreen : MonoBehaviour, IUIScreen {

    public Image Darkening;
    public RectTransform Panel;

    public bool IsVisible {
        get {
            return Panel.gameObject.activeInHierarchy;
        }
        set {
            Darkening.enabled = value;
            Panel.gameObject.SetActive(value);
        }
    }

    void Awake() {
        if (Darkening == null)
            Darkening = GetComponent<Image>();
        if (Panel == null)
            Panel = (RectTransform) transform.Find("Panel");
    }

    void Update() {

    }

}
