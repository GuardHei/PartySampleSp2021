using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {
    public int attackType;
    public float delay;
    public float attackLength;
    public float cooldownLength;
    public bool ignoreArmor;
    private bool cooldown;
    
    protected static readonly int ParamAttackType = Animator.StringToHash("AttackType");

    public void Attack() {
        if (!cooldown) {
            StartCoroutine(AttackCoroutine());
            StartCoroutine(CooldownCoroutine());
        }
    }
    
    public IEnumerator AttackCoroutine() {
        yield return new WaitForSeconds(delay);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Animator>().SetInteger(ParamAttackType, attackType);
        GetComponent<HitboxController>().Attack(ignoreArmor);
        yield return new WaitForSeconds(attackLength);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Animator>().SetInteger(ParamAttackType, 0);
        onAttackCompletion();
    }

    public IEnumerator CooldownCoroutine() {
        cooldown = true;
        yield return new WaitForSeconds(cooldownLength);
        onCooldownCompletion();
        cooldown = false;
    }

    public virtual void onAttackCompletion() {
        //Optionally, override in child. Executes after attack frames finish.
    }

    public virtual void onCooldownCompletion() {
        //Optionally, override in child. Executes after cooldown frames finish.
    }
}