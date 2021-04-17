using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushAttack : MeleeAttack
{
    public KeyCode attackButton = KeyCode.J;

    public void Update() {
        if (Input.GetKeyDown(attackButton)) {
            Attack();
        }
    }
}

