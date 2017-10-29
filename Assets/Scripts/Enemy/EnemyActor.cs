using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : MonoBehaviour, DeathHandler, IActor {

	public List<ItemDrop> itemDrops = new List<ItemDrop>();	
	private HealthComponent healthComponent;
	private AiComponent aiComponent;

    private float lookAngle = 0f;
    private float distanceToTarget = 0f;
    public Animator anim;
    private AILerp lerp;
    public Vector2 LookAngle => Vector2.right;
    public Animator Animator => anim;

    // UNITY
    void Awake() {
        if (anim == null) {
            anim = GetComponent<Animator>();
            if(anim == null){
                anim = GetComponentInChildren<Animator>();
            }
        }
		healthComponent = gameObject.GetOrCreateComponent<HealthComponent>();
		aiComponent = gameObject.GetOrCreateComponent<EnemyAI>();
        lerp = gameObject.GetComponent<AILerp>();
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
    void Update() {
        if(lerp.target != null) {
            Vector2 toVector2 = new Vector2(lerp.target.transform.position.x, lerp.target.transform.position.y);
            Vector2 fromVector2 = new Vector2(this.transform.position.x, this.transform.position.y);

            lookAngle = Vector2.Angle(fromVector2, toVector2) + 45f;
            Vector3 cross = Vector3.Cross(fromVector2, toVector2);

            if (cross.z > 0){
                lookAngle = 360 - lookAngle;    
            }

            distanceToTarget = Mathf.Abs(toVector2.magnitude - fromVector2.magnitude);
        }

        anim.SetFloat("LookDirection", lookAngle);
        anim.SetFloat("Distance", distanceToTarget);
    }
}
