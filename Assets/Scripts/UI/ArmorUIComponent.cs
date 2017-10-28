using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorUIComponent : MonoBehaviour {

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
        rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            ((RectTransform) transform.parent).sizeDelta.x * Health.maxArmor / (Health.maxHealth + Health.maxArmor)
        );
        rectTransform.anchoredPosition = new Vector2(
            Mathf.Lerp(((RectTransform) transform.parent).sizeDelta.x * 0.75f, ((RectTransform) transform.parent).sizeDelta.x * 0.5f, Health.maxArmor / 100f),
            0f
        );

        Panel.rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            ((RectTransform) Panel.transform.parent).sizeDelta.x * Health.currentArmor / Health.maxArmor
        );
        Text.text = $"AP: {Health.currentArmor} / {Health.maxArmor}";
		Text.enabled = Health.maxArmor > 0;			
    }

}