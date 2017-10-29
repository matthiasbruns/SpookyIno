using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperMeleeAttackState : AiState {
    
    public float maxOffset = 1f; 
    public float minAttackDistance = 2f; 
    public float teleportCooldown = 2.5f;
    private float teleportTimer = 0.0f;
    public float attackCooldown = 0.5f;
    private float attackTimer = 0.0f;
    public int teleportCount = 3;
    private int teleportCounter = 0;
    public int hitDamage = 10;

    public AudioClip tauntClip;

    private GameObject player;
    private HasHealth playerHealth;
    private HasMovementAi movementAI;
    private bool isInRange = false;
    private HasAnimator animator;
    private HasAudioSource audioSource;

    public override void OnEnter(GameObject owner){
        base.OnEnter(owner);

        audioSource = owner.GetComponent<HasAudioSource>();
        animator = owner.GetComponent<HasAnimator>();
        if(animator != null) {
            animator.Animator.SetBool(AnimatorFields.STATE_MELEE_ATTACK, true);
        }

        UpdatePlayer();

        List<HasMovementAi> movementAIs;
        owner.GetInterfaces<HasMovementAi>(out movementAIs);

        if(movementAIs == null || movementAIs.Count == 0) {
            Debug.LogError("Cannot wander without HasMovementAi interface");
        } else {
            movementAI = movementAIs[0];
            movementAI.Target = null;
        }

        teleportCounter = teleportCount;

        attackTimer = 0;
        teleportTimer = 0;
    }

    public override void OnExit(GameObject owner){
        base.OnExit(owner);
        if(animator != null) {
            animator.Animator.SetBool(AnimatorFields.STATE_MELEE_ATTACK, false);
        }
    }

    public override void Tick(GameObject owner){   
        if (player != null) {
            if (IsInAttackRange(owner)) {
                isInRange = true;
            } else {
                isInRange = false;
            }
        }

        if (!isInRange){
            Teleport(owner);
        } else {
            Attack(owner);
        }
    }

    private void UpdatePlayer(){
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
        List<HasHealth> healths;
        player.GetInterfaces<HasHealth>(out healths);
        playerHealth = healths[0];
    }

    private void Teleport(GameObject owner){
        teleportTimer -= Time.deltaTime;
        if(teleportTimer <= 0.0f){
            teleportTimer = teleportCooldown;
            bool left = Random.value < 0.5f;
            bool up = Random.value < 0.5f;
            var offset = new Vector3((up ? -1.0f : 1.0f) * maxOffset, (left? -1.0f : 1.0f) * maxOffset);
            var target = player.transform.position + offset;
            movementAI.Enabled = false;
            owner.transform.position = target;
            attackTimer = attackCooldown;

            if(--teleportCounter<= 0){
                isTransitionAllowed = true;
            }
        }
    }

    private bool IsInAttackRange(GameObject owner){
        return Vector2.Distance(owner.transform.position.vec2(), player.transform.position.vec2()) <= minAttackDistance;
    }

    private void Attack(GameObject owner) {
        attackTimer -= Time.deltaTime;
        if(attackTimer <= 0){
            attackTimer = attackCooldown;

            // Hit
            if(playerHealth== null){
                UpdatePlayer();
            }

            animator.Animator.SetTrigger(AnimatorFields.TRIGGER_MELEE_ATTACK);
            if (IsInAttackRange(owner)) {
                playerHealth.ApplyDamage(hitDamage);
                if(audioSource != null) {
                    audioSource.Source.PlayOneShot(tauntClip, 0.5f);
                }
            } 
            movementAI.Enabled = true;
            teleportTimer = teleportCooldown;
        }
    }
}