using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class RangedAttack : MonoBehaviour
{
    public GameObject rangedAttackerPrefab; // unit that spawns and looks like it attacks
    public bool isAttacking;
    public int damage;
    public int paintCost; // not checked for as of yet
    public int attackRadius; // radius of the attack.
    public int attackDelay;
    public int attackRange; // a valid attack must be within this distance
    public float cooldownLength;
    public float cd = 0;
    public KeyCode attackButton = KeyCode.Mouse0; // Currently left mouse button
    private Vector2 markerCoordinates; // holds the coordinates to strike
    private Vector2 screenBounds; // so that we can spawn this attacker to a side of the screen

    private void Start()
    {
        screenBounds =
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Update()
    {
        if (Input.GetKeyDown(attackButton))
        {
            isAttacking = false;
            markerCoordinates = getCoordinatesFromMouse();
            if (!targetIsInRange(markerCoordinates))
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
                isAttacking = true;
                cd = cooldownLength;
            }
        }

        if (isAttacking)
        {
            spawnAttacker();
            StartCoroutine(delayedAttackRoutine());
            isAttacking = false;
        }

        if (cd > 0)
        {
            cd -= Time.deltaTime;
        }
    }

    /**
     * After the delay, spawn a regional attack on the location.
     * Currently, it would also damage the player.
     * TODO: need to test this overlap sphere method. Seems like it should do the job but something is off.
     *
     * @source https://docs.unity3d.com/ScriptReference/Physics.OverlapSphere.html
     * @source https://answers.unity.com/questions/454590/c-how-to-check-if-a-gameobject-contains-a-certain.html
     */
    IEnumerator delayedAttackRoutine()
    {
        yield return new WaitForSeconds(attackDelay);
        Debug.Log("The attack now lands!");
        Collider[] hitColliders = Physics.OverlapSphere(markerCoordinates, attackRadius);
        foreach (var hitCollider in hitColliders)
        {
            Debug.Log("This guy is doomed: " + hitCollider.name + " takes " + damage + " damage.");
            Health hp = hitCollider.GetComponent<Health>();
            if (hp != null)
            {
                hp.Hit(damage);
            }
        }
    }

    /**
     * Spawns a unit to the right of the screen.
     * The unit itself determines when it is deleted. It is not based off of the delay time here.
     * 
     * @source https://youtu.be/E7gmylDS1C4?t=434
     */
    private void spawnAttacker()
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
    private bool targetIsInRange(Vector2 targetCoords)
    {
        Vector2 playerCoords = transform.position;
        float distance = Vector2.Distance(playerCoords, targetCoords);
        return (distance <= attackRange);
    }

    /**
     * Returns the position in the world where the player's mouse is hovering over.
     * 
     * @source https://stackoverflow.com/questions/46998241/getting-mouse-position-in-unity
     */
    private Vector2 getCoordinatesFromMouse()
    {
        Vector3 mousePos = Input.mousePosition; // mouse position in pixels
        Vector2 screenPos = new Vector2(mousePos.x, mousePos.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        return worldPos;
    }
}