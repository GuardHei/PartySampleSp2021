using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallMonoBehaviour : MonoBehaviour
{
    public int damage = 2;
    public float movespeed = 4.0f;
    public float countdownTimer = 10.0f;

    void Update()
    {
        Transform trns = transform;
        trns.position += Time.deltaTime * movespeed * trns.up;
        if (countdownTimer < 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        countdownTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.CompareTag("Player"))
        {
            if (target.TryGetComponent(out Health health))
            {
                health.Hit(damage);
            }
        }
    }
}
