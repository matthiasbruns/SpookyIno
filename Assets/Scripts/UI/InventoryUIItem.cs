using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventoryUIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public InventoryItem ForceItem;
    public InventorySlot Slot;
    public InventoryItem Item => ForceItem != null ? ForceItem : Slot?.item;

    private Image icon;

    void Awake() {
        icon = GetComponent<Image>();
    }

    void Update() {
        if (Item != null) {
            icon.sprite = Item.itemIcon;
            icon.color = Color.white;
        } else {
            // Can't be disabled, as it needs to keep being a raycast target.
            icon.sprite = null;
            icon.color = Color.clear;
        }
    }

    private bool dragging = false;
    private Transform preDragParent;
    private Vector3 preDragPosition;

    public void OnBeginDrag(PointerEventData eventData) {
        if (Item == null)
            return;

        dragging = true;
        preDragParent = transform.parent;
        preDragPosition = transform.localPosition;
        while (transform.parent.GetComponent<Canvas>() == null)
            transform.SetParent(transform.parent.parent, true);
    }

    public void OnDrag(PointerEventData eventData) {
        if (!dragging)
            return;

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (!dragging)
            return;

        transform.SetParent(preDragParent, true);
        transform.localPosition = preDragPosition;
        dragging = false;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (RaycastResult result in results) {
            IUIItemDropHandler handler = result.gameObject.GetComponent<IUIItemDropHandler>();
            if (handler != null && ((Component) handler).transform != transform) {
                handler.OnDrop(this, eventData);
                break;
            }
        }

    }

}
