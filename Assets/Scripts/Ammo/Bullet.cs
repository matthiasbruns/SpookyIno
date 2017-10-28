using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public int hitDamage = 1;
	public SortedSet<string> ignoreTags = new SortedSet<string>();

	void Awake(){
		Rigidbody2D rbody;
		if((rbody = gameObject.GetComponent<Rigidbody2D>()) == null){
			rbody = gameObject.AddComponent<Rigidbody2D>();
			rbody.isKinematic = true;
		}

		List<Collider2D> colliders;
		gameObject.GetInterfaces<Collider2D>(out colliders);
		if(colliders == null || colliders.Count == 0){
			var col = gameObject.AddComponent<CircleCollider2D>();
			colliders = new List<Collider2D>();
			colliders.Add(col);
			col.isTrigger = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(ignoreTags.Contains(other.gameObject.tag)) return;

		List<HasHealth> hasHealths;
        other.gameObject.GetInterfaces<HasHealth>(out hasHealths);

		if(hasHealths == null){
			return;
		}

		foreach(HasHealth hasHealth in hasHealths){
			if(hasHealth.CanBeDamaged){
				hasHealth.ApplyDamage(hitDamage);
			}
		}

		Destroy(gameObject);
    }
}