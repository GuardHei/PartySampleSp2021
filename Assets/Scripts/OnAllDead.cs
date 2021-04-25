using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnAllDead : MonoBehaviour
{
    public List<GameObject> entities = new List<GameObject>();
    public bool invoked = false;

    public UnityEvent onAllDead;

    void Update()
    {
        if (!invoked && entities != null) {
            int deadCount = 0;
            for (int i = 0; i < entities.Count; i++) {
                if (entities[i] == null) {
                    deadCount++;
                }
            }
            if (deadCount == entities.Count) {
                onAllDead.Invoke();
                invoked = true;
            }
        }
    }
}
