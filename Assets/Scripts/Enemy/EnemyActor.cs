using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : MonoBehaviour, DeathHandler, IActor {

	public List<ItemDrop> itemDrops = new List<ItemDrop>();	
	private HealthComponent healthComponent;
	private AiComponent aiComponent;
    
    public Vector2 LookAngle => transform.right; // TODO: Werden Enemies auch "Hände" haben?

    // UNITY
    void Awake() {
		healthComponent = gameObject.GetOrCreateComponent<HealthComponent>();
		aiComponent = gameObject.GetOrCreateComponent<EnemyAI>();
    }

    void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.tag.GetHashCode() == Tags.PLAYER_HASH){
			// Collision with player = damage
			healthComponent.ApplyDamage(1000);
		}
	}

	IEnumerator DropItem(ItemDrop drop){
		var item = GameManager.Instance.itemDatabase.itemList[drop.item];
		var spawned = GameObject.Instantiate(item.itemObject);
		spawned.AddForce(new Vector2(Random.value, Random.value));
		yield return null;
	}

    public void HandleDeath() {
        foreach(ItemDrop drop in itemDrops){
			if(drop.chance > Random.value){
				StartCoroutine(DropItem(drop));
			}
		}
    }
}
