using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerState : AiState {
    public float minDistance = 2f;
    public float attackCooldown = 2.5f;
    public int hitDamage = 10;
    private float attackTimer = 0.0f;
    private GameObject player;
    private HasHealth playerHealth;
    private List<HasMovementAi> movementAIs;
    private bool isInRange = false;
    public override void Tick(GameObject owner)
    {   
        if(movementAIs == null){
            owner.GetInterfaces<HasMovementAi>(out movementAIs);
        }

        if (player == null) {
            UpdatePlayer();
            if(player == null) {
                isTransitionAllowed = true;
            }
        } else {
            if(movementAIs == null || movementAIs.Count == 0) {
                Debug.LogError("Cannot wander without HasMovementAi interface");
            } else {
                HasMovementAi movement = movementAIs[0];
                movement.Target = player.transform;
            }
        }

        if (Vector2.Distance(owner.transform.position.vec2(), player.transform.position.vec2()) <= minDistance) {
            isInRange = true;
        } else {
            isInRange = false;
        }

        if (isInRange){
            Attack();
        }
    }

    private void UpdatePlayer(){
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
        List<HasHealth> healths;
        player.GetInterfaces<HasHealth>(out healths);
        playerHealth = healths[0];
    }

    private void Attack() {
        attackTimer -= Time.deltaTime;
        if(attackTimer <= 0){
            attackTimer = attackCooldown;

            // Hit
            if(playerHealth== null){
                UpdatePlayer();
            }
            playerHealth.ApplyDamage(hitDamage);
            if(playerHealth.Health <= 0){
                isTransitionAllowed = true;
            }
        }
    }
}