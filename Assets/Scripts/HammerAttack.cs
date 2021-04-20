using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAttack : MeleeAttack
{
    public int paintCost;
    public KeyCode attackButton = KeyCode.Mouse1;
    public GameObject player;
    private PlayerController playerController;
    private Transform playerTransform;
    private Vector3 playerPoint;
    private Vector2 mousePoint;
    private bool usesPaint;
    private PaintResource paintResource;

    void Start() {
        playerTransform = player.GetComponent<Transform>();
        playerController = player.GetComponent<PlayerController>();
        usesPaint = player.TryGetComponent(out PaintResource pr);
        if (usesPaint) {
            paintResource = pr;
        }
    }

    void Update() {
        if (Input.GetKeyDown(attackButton)) {
            SpecialAttack();
        }
    }

    public void SpecialAttack() {
        bool enoughPaint = true;
        if (usesPaint) {
            enoughPaint = paintResource.SubPaint(paintCost);
        }
        if (enoughPaint) {
            playerController.inControl = false;
            playerPoint = playerTransform.position;
            mousePoint = GetCoordinatesFromMouse();
            float xdiff = mousePoint.x - playerPoint.x;
            float ydiff = mousePoint.y - playerPoint.y;
            if (Mathf.Abs(xdiff) >= Mathf.Abs(ydiff)) {
                if (xdiff >= 0) {
                    playerController.Turn(Direction.RIGHT);
                } else {
                    playerController.Turn(Direction.LEFT);
                }
            } else {
                if (ydiff >= 0) {
                    playerController.Turn(Direction.UP);
                } else {
                    playerController.Turn(Direction.DOWN);
                }
            }
            Attack();
        }
    }

    /**
     * Returns the position in the world where the player's mouse is hovering over.
     * Copied from RangedAttack.cs.
     */
    public static Vector2 GetCoordinatesFromMouse() {
        Vector3 mousePos = Input.mousePosition; // mouse position in pixels
        Vector2 screenPos = new Vector2(mousePos.x, mousePos.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        return worldPos;
    }

    public override void onAttackCompletion() {
        playerController.inControl = true;
    }
}
