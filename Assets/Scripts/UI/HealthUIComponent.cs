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
        int armor = Mathf.Max(40, Health.maxArmor);
        rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            ((RectTransform) transform.parent).sizeDelta.x * Health.maxHealth / (Health.maxHealth + armor)
        );
        rectTransform.anchoredPosition = new Vector2(
            ((RectTransform) transform.parent).sizeDelta.x * armor / (Health.maxHealth + armor) * Mathf.Max(0f, armor - Health.maxArmor) / 40 * 0.5f,
            0f
        );

        Panel.rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            ((RectTransform) Panel.transform.parent).sizeDelta.x * Health.currentHealth / Health.maxHealth
        );
        Text.text = $"HP: {Health.currentHealth} / {Health.maxHealth}";
    }

}
