using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEdgeUIComponent : MonoBehaviour {

    RectTransform rectTransform;

    public EquipmentComponent Equipment;
    public Image IconLeft;
    public Image IconRight;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();

        // Whoops I did it again.
        if (Equipment == null)
            Equipment = FindObjectOfType<EquipmentComponent>();
        if (IconLeft == null)
            IconLeft = transform.Find("IconLeft").GetComponent<Image>();
        if (IconRight == null)
            IconRight = transform.Find("IconRight").GetComponent<Image>();
    }

    void Update() {
        WeaponSlots slots = Equipment.WeaponSlots;

        if (IconLeft.enabled = (slots.leftHand != null)) {
            IconLeft.sprite = slots.leftHand.itemIcon;
        }

        if (IconRight.enabled = (slots.rightHand != null)) {
            IconRight.sprite = slots.rightHand.itemIcon;
        }
    }

}
