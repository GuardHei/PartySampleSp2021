using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class RangedAttack : MonoBehaviour
{
    public GameObject rangedAttackerPrefab; // unit that spawns and looks like it attacks
    public int damage;
    public int paintCost; // not checked for as of yet
    public int attackRadius; // radius of the attack.
    public int attackDelay;
    public int range; // a valid attack must be within this distance
    public float cooldownLength;
    public KeyCode attackButton = KeyCode.Mouse1; // Currently Right Mouse Button
    private Vector2 attackPoint; // holds the coordinates to strike
    private float cd = 0; // time left until ability is ready for use
    private Vector2 screenBounds; // so that we can spawn this attacker to a side of the screen

    void Update()
    {
        if (Input.GetKeyDown(attackButton))
        {
            attackPoint = GETCoordinatesFromMouse();
            if (!TargetIsInRange(attackPoint))
            {
                Debug.Log("Target out of range.");
            }
            else if (cd > 0)
            {
                Debug.Log("Ranged attack on cooldown.");
            }
            else
            {
                Debug.Log("Attack start.");
                cd = cooldownLength;
                screenBounds = GETScreenBounds();
                SpawnAttacker();
                StartCoroutine(DelayedAttackRoutine());
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
     * @source https://youtu.be/E7gmylDS1C4?t=434
     */
    private void SpawnAttacker()
    {
        GameObject a = Instantiate(rangedAttackerPrefab) as GameObject;
        a.transform.position = new Vector2(screenBounds.x - 1, screenBounds.y - 1);
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
    private static Vector2 GETCoordinatesFromMouse()
    {
        Vector3 mousePos = Input.mousePosition; // mouse position in pixels
        Vector2 screenPos = new Vector2(mousePos.x, mousePos.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        return worldPos;
    }

    private static Vector2 GETScreenBounds()
    {
        return
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    /**
     * Copy pasted from HitBoxController.cs since I didn't see a way to get a transform
     * object given a Vector2D. Also thought it would be too hacky to somehow make a copy of
     * the current transform, modify it, then pass it in.
     */
    private void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint, attackRadius);
        foreach (var hit in hits)
        {
            if (hit == null) break;
            if (hit.TryGetComponent(out Health health)) health.Hit(damage);
        }
    }
}