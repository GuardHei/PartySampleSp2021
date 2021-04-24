using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class GymBossMonoBehaviour : MonoBehaviour
{

    public Health health;
    public int powerSources = 0;
    
    [Header("Distances")]
    public float longDistance = 4.0f;
    public float shortDistance = 2.0f;
    
    [Header("References")]
    public GameObject target;
    public GameObject smashPrefab;
    public GameObject dodgeballPrefab;
    public GameObject firewallPrefab;
    public Collider2D[] unreachableAreas;

    [Header("Attributes")] 
    public float moveSpeed = 6.0f;
    public float chargeSpeed = 10.0f;
    public int smashDamage = 3;
    public int chargeDamage = 2;
    public int dodgeballDamage = 1;
    public float firewallYOffset = 10.0f;
    public int firewallDamage = 2;

    [Header("Probabilities")] 
    [Range(0, 1.0f)] public float long_move = 0.0f;
    [Range(0, 1.0f)] public float long_charge = 0.0f;
    [Range(0, 1.0f)] public float long_dodgeball = 0.0f;
    [Range(0, 1.0f)] public float long_firewall = 0.0f;
    [Range(0, 1.0f)] public float long_smash = 0.0f;
    [Range(0, 1.0f)] public float long_spin = 0.0f;
    
    [Range(0, 1.0f)] public float medium_move = 0.0f;
    [Range(0, 1.0f)] public float medium_charge = 0.0f;
    [Range(0, 1.0f)] public float medium_dodgeball = 0.0f;
    [Range(0, 1.0f)] public float medium_firewall = 0.0f;
    [Range(0, 1.0f)] public float medium_smash = 0.0f;
    [Range(0, 1.0f)] public float medium_spin = 0.0f;
    
    [Range(0, 1.0f)] public float short_move = 0.0f;
    [Range(0, 1.0f)] public float short_charge = 0.0f;
    [Range(0, 1.0f)] public float short_dodgeball = 0.0f;
    [Range(0, 1.0f)] public float short_firewall = 0.0f;
    [Range(0, 1.0f)] public float short_smash = 0.0f;
    [Range(0, 1.0f)] public float short_spin = 0.0f;
    
    protected Collider2D collider;
    protected Collider2D targetCollider;
    protected Health targetHealth;
    protected Rigidbody2D rigidBody;
    protected Animator animator;
    protected int actionState = Animator.StringToHash("ActionState");
    protected int horizontalVelocity = Animator.StringToHash("HorizontalVelocity");

    protected enum ActionState
    {
        IDLE = 0, WALKING = 1, CHARGING = 2, SMASHING = 3, FIREWALL = 4, SPINNING = 5, DODGEBALL_AP = 6,
        DODGEBALL_ATTACK = 7, DODGEBALL_BACKSWING = 8
    }

    protected enum DistanceLevel
    {
        SHORT, MEDIUM, LONG
    }

    protected DistanceLevel GetDistanceLevel()
    {
        float distanceToTarget = (target.transform.position - transform.position).magnitude;
        if (distanceToTarget > longDistance) return DistanceLevel.LONG;
        if (distanceToTarget > shortDistance) return DistanceLevel.MEDIUM;
        return DistanceLevel.SHORT;
    }


    protected void Awake()
    {
        collider = GetComponent<Collider2D>();
        targetCollider = target.GetComponent<Collider2D>();
        targetHealth = target.GetComponent<Health>();
        health = GetComponent<Health>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetInteger(actionState, (int) ActionState.IDLE);
        animator.SetFloat(horizontalVelocity, 0);
    }
    protected void Start()
    {
        DetermineNextAction();
    }

    protected void DetermineNextAction()
    {
        bool unreachable = false;
        foreach (Collider2D unreachableArea in unreachableAreas)
        {
            if (unreachableArea.bounds.Contains(target.transform.position))
            {
                unreachable = true;
            }
        }
        if (unreachable)
        {
            Delayed(1.0f, FirewallAttackCoroutine());
            return;
        }
        switch (GetDistanceLevel())
        {
            case DistanceLevel.LONG:
                DetermineLongAction();
                return;
            case DistanceLevel.MEDIUM:
                DetermineMediumAction();
                return;
            case DistanceLevel.SHORT:
                DetermineShortAction();
                return;
        }
    }

    protected void DetermineLongAction()
    {
        float rand = Random.Range(0, 1.0f);
        float total_prob = long_move + long_charge + long_dodgeball + long_firewall + long_smash + long_spin;
        float accumulated_prob = 0.0f;
        accumulated_prob += long_move / total_prob;
        if (rand < accumulated_prob)
        {
            NavigateTowardsTarget();
            return;
        }
        accumulated_prob += long_charge / total_prob;
        if (rand < accumulated_prob)
        {
            ChargeAttack();
            return;
        }
        accumulated_prob += long_dodgeball / total_prob;
        if (rand < accumulated_prob)
        {
            DodgeballAttack();
            return;
        }
        accumulated_prob += long_firewall / total_prob;
        if (rand < accumulated_prob)
        {
            FirewallAttack();
            return;
        }
        accumulated_prob += long_smash / total_prob;
        if (rand < accumulated_prob)
        {
            SmashAttack();
            return;
        }
        accumulated_prob += long_spin / total_prob;
        if (rand < accumulated_prob)
        {
            SpinAttack();
            return;
        }
    }
    
    protected void DetermineMediumAction()
    {
        float rand = Random.Range(0, 1.0f);
        float total_prob = medium_move + medium_charge + medium_dodgeball + medium_firewall + medium_smash + medium_spin;
        float accumulated_prob = 0.0f;
        accumulated_prob += medium_move / total_prob;
        if (rand < accumulated_prob)
        {
            NavigateTowardsTarget();
            return;
        }
        accumulated_prob += medium_charge / total_prob;
        if (rand < accumulated_prob)
        {
            ChargeAttack();
            return;
        }
        accumulated_prob += medium_dodgeball / total_prob;
        if (rand < accumulated_prob)
        {
            DodgeballAttack();
            return;
        }
        accumulated_prob += medium_firewall / total_prob;
        if (rand < accumulated_prob)
        {
            FirewallAttack();
            return;
        }
        accumulated_prob += medium_smash / total_prob;
        if (rand < accumulated_prob)
        {
            SmashAttack();
            return;
        }
        accumulated_prob += medium_spin / total_prob;
        if (rand < accumulated_prob)
        {
            SpinAttack();
            return;
        }
    }
    
    protected void DetermineShortAction()
    {
        float rand = Random.Range(0, 1.0f);
        float total_prob = short_move + short_charge + short_dodgeball + short_firewall + short_smash + short_spin;
        float accumulated_prob = 0.0f;
        accumulated_prob += short_move / total_prob;
        if (rand < accumulated_prob)
        {
            NavigateTowardsTarget();
            return;
        }
        accumulated_prob += short_charge / total_prob;
        if (rand < accumulated_prob)
        {
            ChargeAttack();
            return;
        }
        accumulated_prob += short_dodgeball / total_prob;
        if (rand < accumulated_prob)
        {
            DodgeballAttack();
            return;
        }
        accumulated_prob += short_firewall / total_prob;
        if (rand < accumulated_prob)
        {
            FirewallAttack();
            return;
        }
        accumulated_prob += short_smash / total_prob;
        if (rand < accumulated_prob)
        {
            SmashAttack();
            return;
        }
        accumulated_prob += short_spin / total_prob;
        if (rand < accumulated_prob)
        {
            SpinAttack();
            return;
        }
    }

    public void NavigateTowardsTarget()
    {
        StopAllCoroutines();
        rigidBody.velocity = Vector2.zero;
        animator.SetFloat(horizontalVelocity, rigidBody.velocity.x);
        StartCoroutine(NavigateTowardsTargetCoroutine());
    }

    public IEnumerator NavigateTowardsTargetCoroutine()
    {
        
        animator.SetInteger(actionState, (int) ActionState.WALKING);
        
        DistanceLevel originalDistanceLevel = GetDistanceLevel();
        float addedMoveSpeed = 0.0f;
        while (GetDistanceLevel() == originalDistanceLevel && Random.Range(0.0f, 1.0f) < 0.99f)
        {
            Vector2 displacement = target.transform.position - transform.position;
            rigidBody.velocity = (moveSpeed + addedMoveSpeed) * displacement.normalized;
            animator.SetFloat(horizontalVelocity, rigidBody.velocity.x);
            addedMoveSpeed += Time.deltaTime * moveSpeed;
            yield return null;
        }
        DetermineNextAction();
    }

    public void SmashAttack()
    {
        StopAllCoroutines();
        rigidBody.velocity = Vector2.zero;
        StartCoroutine(SmashAttackCoroutine());
    }

    public IEnumerator SmashAttackCoroutine()
    {
        
        animator.SetInteger(actionState, (int) ActionState.SMASHING);
        
        // PARAMS
        float attackPointCountdown = 0.8f;
        float backswingCountdown = 0.2f;
        
        // ATTACK
        GameObject smash = Instantiate(smashPrefab);
        smash.transform.position = target.transform.position;
        SmashMonoBehaviour smb = smash.GetComponent<SmashMonoBehaviour>();
        smb.damage = smashDamage;

        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        
        // ATTACK POINT
        smb.countdownTimer = attackPointCountdown;
        while (attackPointCountdown > 0.0f)
        {
            attackPointCountdown -= Time.deltaTime;
            yield return null;
        }
        
        // ATTACK BACKSWING
        while (backswingCountdown > 0.0f)
        {
            backswingCountdown -= Time.deltaTime;
            yield return null;
        }

        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        DetermineNextAction();
    }
    
    public void DodgeballAttack()
    {
        StopAllCoroutines();
        rigidBody.velocity = Vector2.zero;
        animator.SetFloat(horizontalVelocity, rigidBody.velocity.x);
        StartCoroutine(DodgeballAttackCoroutine());
    }

    public IEnumerator DodgeballAttackCoroutine()
    {
        // PARAMS
        float attackPointCountdown = 0.4f;
        int numToThrow = Random.Range(4, 6);
        float projectileSpeed = 8.0f;
        float throwInterval = 0.15f;
        float backswingCountdown = 0.4f;
        
        // ATTACK POINT
        animator.SetInteger(actionState, (int) ActionState.DODGEBALL_AP);
        Vector2 displacement_direction = (target.transform.position - transform.position).normalized;
        rigidBody.velocity = -chargeSpeed * displacement_direction;
        animator.SetFloat(horizontalVelocity, rigidBody.velocity.x);
        while (attackPointCountdown > 0.0f)
        {
            attackPointCountdown -= Time.deltaTime;
            yield return null;
        }
        
        // ATTACK
        animator.SetInteger(actionState, (int) ActionState.DODGEBALL_ATTACK);
        while (numToThrow > 0)
        {
            for (int spawn = 0; spawn < 1; spawn += 1)
            {
                displacement_direction = (target.transform.position - transform.position).normalized;
                GameObject dodgeball = Instantiate(dodgeballPrefab);
                dodgeball.transform.position = transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
                dodgeball.transform.rotation = Quaternion.LookRotation(Vector3.forward, displacement_direction);
                dodgeball.transform.Rotate(Vector3.forward, Random.Range(-15, 15));
                DodgeballMonoBehaviour dbmb = dodgeball.GetComponent<DodgeballMonoBehaviour>();
                dbmb.damage = dodgeballDamage;
                dbmb.movespeed = projectileSpeed;
            }

            numToThrow -= 1;
            yield return new WaitForSeconds(throwInterval);
        }
        
        // BACKSWING
        animator.SetInteger(actionState, (int) ActionState.DODGEBALL_BACKSWING);
        while (backswingCountdown > 0.0f)
        {
            backswingCountdown -= Time.deltaTime;
            yield return null;
        }
        
        DetermineNextAction();
    }
    
    public void ChargeAttack()
    {
        StopAllCoroutines();
        rigidBody.velocity = Vector2.zero;
        StartCoroutine(ChargeAttackCoroutine());
    }
    
    public IEnumerator ChargeAttackCoroutine()
    {
        animator.SetInteger(actionState, (int) ActionState.CHARGING);
        Vector3 displacement = target.transform.position - transform.position;
        float chargeCountdown = (displacement.magnitude / chargeSpeed) + 0.4f;
        displacement.Normalize();
        while (chargeCountdown > 0.0f)
        {
            if (targetCollider.IsTouching(collider))
            {
                targetHealth?.Hit(chargeDamage);
            }
            rigidBody.velocity = chargeSpeed * displacement.normalized;
            animator.SetFloat(horizontalVelocity, rigidBody.velocity.x);
            chargeCountdown -= Time.deltaTime;
            yield return null;
        }
        DetermineNextAction();
    }
    
    public void FirewallAttack()
    {
        StopAllCoroutines();
        rigidBody.velocity = Vector2.zero;
        animator.SetFloat(horizontalVelocity, rigidBody.velocity.x);
        StartCoroutine(FirewallAttackCoroutine());
    }
    
    public IEnumerator FirewallAttackCoroutine()
    {
        
        animator.SetInteger(actionState, (int) ActionState.FIREWALL);
        
        // PARAMS
        float attackPointCountdown = 1.6f;
        float backswingCountdown = 0.2f;
        
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        
        // ATTACK POINT
        while (attackPointCountdown > 0.0f)
        {
            attackPointCountdown -= Time.deltaTime;
            yield return null;
        }
        
        // ATTACK
        GameObject firewall = Instantiate(firewallPrefab);
        firewall.transform.position = target.transform.position + firewallYOffset * Vector3.up;
        
        
        // BACKSWING
        while (backswingCountdown > 0.0f)
        {
            backswingCountdown -= Time.deltaTime;
            yield return null;
        }
        
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        DetermineNextAction();
    }

    public void Delayed(float delay, IEnumerator routine)
    {
        StopAllCoroutines();
        rigidBody.velocity = Vector2.zero;
        animator.SetFloat(horizontalVelocity, rigidBody.velocity.x);
        StartCoroutine(DelayedCoroutine(delay, routine));
    }

    public IEnumerator DelayedCoroutine(float delay, IEnumerator routine)
    {
        animator.SetInteger(actionState, (int) ActionState.IDLE);
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(delay);
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidBody.velocity = Vector2.zero;
        animator.SetFloat(horizontalVelocity, rigidBody.velocity.x);
        StartCoroutine(routine);
    }

    public void SpinAttack()
    {
        StopAllCoroutines();
        rigidBody.velocity = Vector2.zero;
        animator.SetFloat(horizontalVelocity, rigidBody.velocity.x);
        StartCoroutine(SpinAttackCoroutine());
    }

    public IEnumerator SpinAttackCoroutine()
    {
        
        animator.SetInteger(actionState, (int) ActionState.SPINNING);
        
        // PARAMS
        float attackPointCountdown = 0.4f;
        int numToThrow = Random.Range(5, 10);
        float projectileSpeed = 6.0f;
        float throwInterval = 0.4f;
        float backswingCountdown = 0.4f;

        // ATTACK POINT
        while (attackPointCountdown > 0.0f)
        {
            attackPointCountdown -= Time.deltaTime;
            yield return null;
        }
        
        // ATTACK
        while (numToThrow > 0)
        {
            for (int spawn = 0; spawn < 12; spawn += 1)
            {
                GameObject dodgeball = Instantiate(dodgeballPrefab);
                dodgeball.transform.position = transform.position;
                dodgeball.transform.Rotate(Vector3.forward, Random.Range(0, 360));
                DodgeballMonoBehaviour dbmb = dodgeball.GetComponent<DodgeballMonoBehaviour>();
                dbmb.movespeed = projectileSpeed;
                dbmb.damage = dodgeballDamage;
            }
            numToThrow -= 1;
            rigidBody.velocity = chargeSpeed * new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            animator.SetFloat(horizontalVelocity, rigidBody.velocity.x);
            yield return new WaitForSeconds(throwInterval);
        }
        
        // BACKSWING
        while (backswingCountdown > 0.0f)
        {
            backswingCountdown -= Time.deltaTime;
            yield return null;
        }
        
        DetermineNextAction();
    }
}