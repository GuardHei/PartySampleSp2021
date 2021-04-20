using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushAttack : MeleeAttack
{
    public KeyCode attackButton = KeyCode.J;
    public GameObject player;
    private PlayerController playerController;
    private Transform playerTransform;
    private Vector3 playerPoint;
    private Vector2 mousePoint;
    
    void Awake() {
        playerTransform = player.transform;
        playerController = player.GetComponent<PlayerController>();
    }

    public void Update() {
        if (Input.GetKeyDown(attackButton)) {
            if (!playerController.inControl) return;
            if (cooldown) return;
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
    
    public override void onAttackCompletion() {
        playerController.inControl = true;
    }
}

