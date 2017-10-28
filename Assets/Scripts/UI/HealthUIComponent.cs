using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIComponent : MonoBehaviour {

    RectTransform rectTransform;

    public HealthComponent Health;
    public Image Panel;
    public Text Text;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();

        // Whoops I did it again.
        if (Health == null)
            Health = FindObjectOfType<HealthComponent>();
        if (Panel == null)
            Panel = transform.Find("Panel").GetComponent<Image>();
        if (Text == null)
            Text = transform.Find("Text").GetComponent<Text>();
    }

    void Update() {
        rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            ((RectTransform) transform.parent).sizeDelta.x * Health.maxHealth / (Health.maxHealth + Health.maxArmor)
        );

        Panel.rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            ((RectTransform) Panel.transform.parent).sizeDelta.x * Health.currentHealth / Health.maxHealth
        );
        Text.text = $"HP: {Health.currentHealth} / {Health.maxHealth}";
    }

}
