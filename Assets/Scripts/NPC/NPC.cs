using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, Interactable {

	protected bool isInteractable = true;
	protected SpriteOutline outline;

	void Awake() {
		outline = gameObject.GetComponent<SpriteOutline>();
	}
    public bool CanUse => isInteractable;

    public bool CanHighlight => isInteractable;

    public virtual void Highlight(GameObject executer, bool activate)
    {
		if(activate) {
			outline.outlineSize = 2;
		} else {
			outline.outlineSize = 0;
		}
    }

    public virtual void Use(GameObject executer)
    {
		Debug.Log("USE", this);
    }
}
