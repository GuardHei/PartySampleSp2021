using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public int delayLength;
    public int frameLength;
    public int cooldownLength;
    public bool ignoreArmor = false;
    private bool cooldown = false;

    public void Attack() {
        if (!cooldown) {
            StartCoroutine("AttackCoroutine");
            StartCoroutine("CooldownCoroutine");
        }
    }
    
    public IEnumerator AttackCoroutine() {
        for (int f = 0; f < delayLength; f += 1) {
            yield return null;
        }
        GetComponent<SpriteRenderer>().enabled = true;
        for (int f = 0; f < frameLength; f += 1) {
            GetComponent<HitboxController>().Attack(ignoreArmor);
            yield return null;
        }
        GetComponent<SpriteRenderer>().enabled = false;
        onAttackCompletion();
    }

    public IEnumerator CooldownCoroutine() {
        cooldown = true;
        for (int f = 0; f < cooldownLength; f += 1) {
            yield return null;
        }
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