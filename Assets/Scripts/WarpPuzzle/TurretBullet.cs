﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class TurretBullet : MonoBehaviour
{

    public int _damage = int.MaxValue;
    public float _movespeed = 10f;

    void Update()
    {
        Transform trns = transform;
        trns.position += Time.deltaTime * _movespeed * trns.up;
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.CompareTag("Player"))
        {
            if (target.TryGetComponent(out Health health))
            {
                health.Hit(_damage);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Destroy(gameObject);
        }
        else if (!target.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
