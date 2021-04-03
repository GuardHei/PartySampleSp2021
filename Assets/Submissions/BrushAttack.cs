using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushAttack : MonoBehaviour
{
	public bool isAttacking;
    public int frameLength;

    public int cooldownLength;

    public int frame;

    public int cd;

    public void Update() {
        if (Input.GetKeyDown(KeyCode.J) && (cd == 0)) {
            Debug.Log("Attack Start");
            isAttacking = true;
            frame = 0;
            cd = cooldownLength;
		    GetComponent<SpriteRenderer>().enabled = true;
        }
        if (isAttacking) {
            GetComponent<HitboxController>().Attack();
            if (frame == frameLength) {
                Debug.Log("Attack End");
                GetComponent<SpriteRenderer>().enabled = false;
                isAttacking = false;
            }
            frame += 1;
        }
        if (cd > 0) {
            cd -= 1;
        }
    }
}

