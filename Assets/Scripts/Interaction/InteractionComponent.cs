using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionComponent : MonoBehaviour {
	public float interactionRadius = 2.5f;
	public LayerMask interactionLayers;
	private List<Interactable> oldInteractablesInRadius = new List<Interactable>();
	private List<Interactable> interactablesInRadius = new List<Interactable>();
	private Interactable interactionTarget = null;

	void Awake(){
		if(interactionLayers == 0){
			interactionLayers = 1 << LayerMask.NameToLayer(Layers.INTERACTION);
		}
	}

	void Update (){
		if(interactionTarget != null) {
			if(Input.GetButtonDown(Keys.USE)){
				if(interactionTarget.CanUse){
					interactionTarget.Use(gameObject);
				}
			}
		}

		// Update highlights
		foreach(Interactable interactable in interactablesInRadius) {
			if(interactable.CanHighlight) {
				interactable.Highlight(gameObject, true);
			}
		}
		foreach(Interactable interactable in oldInteractablesInRadius) {
			if(interactable.CanHighlight && !interactablesInRadius.Contains(interactable)) {
				interactable.Highlight(gameObject, false);
			}
		}
		oldInteractablesInRadius.Clear();
	}

	void FixedUpdate(){
		oldInteractablesInRadius = interactablesInRadius;
		interactablesInRadius = new List<Interactable>();

		Interactable interactable = null;
		float closestDistance = float.MaxValue;

		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, interactionRadius, new Vector2(1,1), 1f, interactionLayers);
		if(hits != null && hits.Length > 0) {
			foreach(RaycastHit2D hit in hits) {
				var distance = Vector2.Distance(transform.position, hit.collider.transform.position);
				if(interactable == null || distance < closestDistance){
					var tmp = hit.collider.gameObject.GetComponent<Interactable>();
					if(tmp != null){
						interactablesInRadius.Add(tmp);
						interactable = tmp;
						closestDistance = distance;
					}
				}
			}
		}

		interactionTarget = interactable;
	}
}
