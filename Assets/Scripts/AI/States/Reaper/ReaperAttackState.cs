using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperAttackState : CompositeAiState {

    private HasAnimator animator;

    public override void OnEnter(GameObject owner){
        base.OnEnter(owner);
        animator = owner.GetComponent<HasAnimator>();
        if(animator != null) {
            animator.Animator.SetBool(AnimatorFields.STATE_ATTACK, true);
        }
    }

    public override void OnExit(GameObject owner){
        if(animator != null) {
            animator.Animator.SetBool(AnimatorFields.STATE_ATTACK, false);
        }
        base.OnExit(owner);
    }

    public override void Tick(GameObject owner) {
        base.Tick(owner);
    }
}