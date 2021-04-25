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

    void Awake() {
        playerTransform = player.transform;
        playerController = player.GetComponent<PlayerController>();
        usesPaint = player.TryGetComponent(out PaintResource pr);
        if (usesPaint) {
            paintResource = pr;
        }
    }

    void Update() {
        if (Input.GetKeyDown(attackButton)) SpecialAttack();
    }

    public void SpecialAttack() {
        if (!playerController.inControl) return;
        if (cooldown) return;
        bool enoughPaint = true;
        if (usesPaint) {
            enoughPaint = paintResource.SubPaint(paintCost);
        }
        if (enoughPaint) {
            playerController.inControl = false;
            playerController.GetComponent<Health>().invincible = true;
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

    public override void onAttackCompletion() {
        playerController.inControl = true;
        playerController.GetComponent<Health>().invincible = false;
    }
}
