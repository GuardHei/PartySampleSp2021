using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour {
    public AudioClip sfx;
    public GameObject rangedAttackerPrefab; // unit that spawns and looks like it attacks
    public GameObject markerPrefab;
    public int damage;
    public int paintCost; // if the user uses paint, subtracts this value from their paint total
    public int attackRadius; // radius of the attack.
    public int attackDelay;
    public int range; // a valid attack must be within this distance
    public float cooldownLength;
    public KeyCode attackButton = KeyCode.Mouse1; // Currently Right Mouse Button
    private Vector2 attackPoint; // holds the coordinates to strike
    [HideInInspector]
    public float cd = 0; // time left until ability is ready for use
    private Vector2 screenBounds; // so that we can spawn this attacker to a side of the screen
    private bool usesPaint; // tracks if the user uses paint, if not they can attack for free
    private PaintResource paintResource; // if the user uses paint, holds the paint resource for quick accessing
    private RangedAttacker activeTarget;
    private RangedAttacker activeAttacker;

    private void Start()
    {
        usesPaint = TryGetComponent(out PaintResource pr);
        if (usesPaint)
        {
            paintResource = pr;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(attackButton))
        {
            var point = GetCoordinatesFromMouse();
            if (!TargetIsInRange(point))
            {
                Debug.Log("Target out of range.");
            }
            else if (cd > 0)
            {
                Debug.Log("Ranged attack on cooldown.");
            }
            else if (usesPaint && (paintResource.paint < paintCost))
            {
                Debug.Log("RANGED_ATTACK: Not enough paint.");
            }
            else
            {
                Debug.Log("Attack start.");
                // subtract paint
                if (usesPaint)
                {
                    paintResource.SubPaint(paintCost);
                }
                attackPoint = point;
                cd = cooldownLength;
                screenBounds = GetScreenBounds();
                SpawnAttacker();
                StartCoroutine(DelayedAttackRoutine());
            }
        }

        /* Delete this if we don't want to do rotation */
        if (activeAttacker != null && activeTarget != null)
        {
            // an attacker has spawned, check to see if we want it to face its target.
            if (activeAttacker.facesTarget)
            {
                activeAttacker.faceTarget(activeTarget.gameObject, activeAttacker.RotationSpeed);
            }
        }

        if (cd > 0)
        {
            cd -= Time.deltaTime;
        }
    }

    /**
     * After the delay, spawn a regional attack on the location. Currently,
     * it should also damage the player.
     * 
     * TODO: need a telegraph to show the player will the attack will land.
     */
    private IEnumerator DelayedAttackRoutine()
    {
        yield return new WaitForSeconds(attackDelay);
        Debug.Log("The attack now lands!");
        Attack();
    }

    /**
     * Spawns a unit and places it on the right side of the screen.
     * The unit itself determines when it is deleted. It is not based off of the delay time here.
     * Can mess around with the transform.position as desired.
     *
     * Note: for funny 3d way of facing target, use:
     * attackerObject.transform.LookAt(targetObject.transform);
     * 
     * @source https://youtu.be/E7gmylDS1C4?t=434
     */
    private void SpawnAttacker()
    {
        GameObject attackerObject = Instantiate(rangedAttackerPrefab);
        GameObject targetObject = Instantiate(markerPrefab);
        activeAttacker = attackerObject.GetComponent<RangedAttacker>();
        activeTarget = targetObject.GetComponent<RangedAttacker>();
        attackerObject.transform.position = new Vector2(screenBounds.x - 1, screenBounds.y - 1);
        activeAttacker.lifespan = attackDelay;
        targetObject.transform.position = new Vector3(attackPoint.x, attackPoint.y, targetObject.transform.position.z);
        activeTarget.lifespan = attackDelay;
        // check if this attacker wants to align horizontally with its target
        if (activeAttacker.alignsWithTargetHorizontally)
        {
            attackerObject.transform.position =
                new Vector2(attackerObject.transform.position.x, targetObject.transform.position.y);
        }

        // check if this attacker wants to align vertically with its target
        if (activeAttacker.alignsWithTargetVertically)
        {
            attackerObject.transform.position =
                new Vector2(targetObject.transform.position.x, attackerObject.transform.position.y);
        }
    }

    /**
     * Checks to see if the player's target is in range.
     *
     * @source https://www.codegrepper.com/code-examples/csharp/
     * unity+how+to+check+if+a+game+object+if+with+in+a+radius
     */
    private bool TargetIsInRange(Vector2 targetCoords)
    {
        Vector2 playerCoords = transform.position;
        float distance = Vector2.Distance(playerCoords, targetCoords);
        return (distance <= range);
    }

    /**
     * Returns the position in the world where the player's mouse is hovering over.
     * 
     * @source https://stackoverflow.com/questions/46998241/getting-mouse-position-in-unity
     */
    private static Vector2 GetCoordinatesFromMouse()
    {
        Vector3 mousePos = Input.mousePosition; // mouse position in pixels
        Vector2 screenPos = new Vector2(mousePos.x, mousePos.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        return worldPos;
    }

    private static Vector2 GetScreenBounds() =>
        Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

    /**
     * Copy pasted from HitBoxController.cs since I didn't see a way to get a transform
     * object given a Vector2D. Also thought it would be too hacky to somehow make a copy of
     * the current transform, modify it, then pass it in.
     *
     * If the user of this script keeps track of a paint cost, then it'll deduct the paint cost from the user's
     * total paint. If the user does not care about paint or paint costs, they can attack for free.
     */
    private void Attack()
    {
        if (sfx) AudioManager.PlayAtPoint(sfx, attackPoint);
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint, attackRadius);
        foreach (var hit in hits)
        {
            if (hit == null) break;
            if (hit.TryGetComponent(out Health health)) health.Hit(damage);
        }
    }
}