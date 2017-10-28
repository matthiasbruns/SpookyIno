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
            Health = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<HealthComponent>();
        if (Panel == null)
            Panel = transform.Find("Panel").GetComponent<Image>();
        if (Text == null)
            Text = transform.Find("Text").GetComponent<Text>();
    }

    void Update() {
        rectTransform.anchoredPosition = new Vector2(
            ((RectTransform) transform.parent).sizeDelta.x * (-Health.maxArmor) / 100f * 0.25f,
            0f
        );

        Panel.rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            ((RectTransform) Panel.transform.parent).sizeDelta.x * Health.currentHealth / Health.maxHealth
        );
        Text.text = $"HP: {Health.currentHealth} / {Health.maxHealth}";
    }

}
