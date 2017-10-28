using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEdgeUIComponent : MonoBehaviour {

    RectTransform rectTransform;

    public EquipmentComponent Equipment;
    public Image Icon;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();

        // Whoops I did it again.
        if (Equipment == null)
            Equipment = FindObjectOfType<EquipmentComponent>();
        if (Icon == null)
            Icon = transform.Find("Icon").GetComponent<Image>();
    }

    void Update() {
        if (Icon.enabled = (Equipment.mainHand != null)) {
            Icon.sprite = Equipment.mainHand.itemIcon;
        }
    }

}
