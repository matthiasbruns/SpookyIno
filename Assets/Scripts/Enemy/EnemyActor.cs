using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : MonoBehaviour {

	public List<ItemDrop> itemDrops = new List<ItemDrop>();

	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.tag.GetHashCode() == Tags.PLAYER){
			// Collision with player = damage
			ApplyDamage(1000);
		}
	}

	public void ApplyDamage(int damage){
		// TODO: REPLACE WITH REAL HEALTH SCRIPT		
		foreach(ItemDrop drop in itemDrops){
			if(drop.chance > Random.value){
				StartCoroutine(DropItem(drop));
			}
		}

		Destroy(gameObject);
	}

	IEnumerator DropItem(ItemDrop drop){
		var item = GameManager.Instance.itemDatabase.itemList[drop.item];
		var spawned = GameObject.Instantiate(item.itemObject);
		spawned.AddForce(new Vector2(Random.value, Random.value));
		yield return null;
	}

}
