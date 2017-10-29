using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemComponent : MonoBehaviour, HasInventoryItem, Interactable {

	[SerializeField]
	private int amount = 1;
    public int Amount => amount;
	
	[SerializeField]
	private int item;
    public int ItemId => item;

	private SpriteOutline[] outlines;

	void Awake(){
		outlines = GetComponentsInChildren<SpriteOutline>();
	}
    public void Use(GameObject executer){ }
    public bool CanUse => false;

    public bool CanHighlight => true;

    public void Highlight(GameObject executer, bool activate) {
		if(activate) {
			foreach(SpriteOutline outlines in outlines){
				outlines.outlineSize = 1;
			}
		} else {
			foreach(SpriteOutline outlines in outlines){
				outlines.outlineSize = 0;
			}
		}
    }
}
