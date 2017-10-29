using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperRangedAttackState : AiState {
    public float attackCooldown = 2.5f;
    public int hitDamage = 10;

    public int shootCountTillTransision = 3;
    private int shootCounter = 0;
    public Bullet projectile;
    public float projectileSpeed = 10.0f;

    public AudioClip attackClip;

    private float attackTimer = 0.0f;
    private GameObject player;
    private HasHealth playerHealth;
    private HasMovementAi movementAI;
    private HasAnimator animator;

    private HasAudioSource audioSource;

    public override void OnEnter(GameObject owner){
        base.OnEnter(owner);

        audioSource = owner.GetComponent<HasAudioSource>();
        animator = owner.GetComponent<HasAnimator>();
        if(animator != null) {
            animator.Animator.SetBool(AnimatorFields.STATE_RANGE_ATTACK, true);
        }

        UpdatePlayer();

        List<HasMovementAi> movementAIs;
        owner.GetInterfaces<HasMovementAi>(out movementAIs);

        if(movementAIs == null || movementAIs.Count == 0) {
            Debug.LogError("Cannot wander without HasMovementAi interface");
        } else {
            movementAI = movementAIs[0];
            movementAI.Target = owner.transform;
        }

        shootCounter = shootCountTillTransision;
    }

    public override void OnExit(GameObject owner){
        base.OnExit(owner);
        if(animator != null) {
            animator.Animator.SetBool(AnimatorFields.STATE_RANGE_ATTACK, false);
        }
    }

    public override void Tick(GameObject owner){   
        Attack(owner);
    }

    private void UpdatePlayer(){
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
        List<HasHealth> healths;
        player.GetInterfaces<HasHealth>(out healths);
        playerHealth = healths[0];
    }

    private void Attack(GameObject owner) {
        attackTimer -= Time.deltaTime;
        if(attackTimer <= 0){
            attackTimer = attackCooldown;

            // Hit
            if(playerHealth== null){
                UpdatePlayer();
            }

            var direction = player.transform.position.vec2() - owner.transform.position.vec2();
            direction = direction.normalized * projectileSpeed;
            
            var bullet = GameObject.Instantiate(projectile, owner.transform.position, Quaternion.identity);
            bullet.ignoreTags.Add(Tags.ENEMY);
            bullet.GetComponent<Rigidbody2D>().velocity = direction;
            audioSource.Source.PlayOneShot(attackClip, 0.5f);

            if(shootCounter-- <= 0){
                isTransitionAllowed = true;
            }
        }
    }
}