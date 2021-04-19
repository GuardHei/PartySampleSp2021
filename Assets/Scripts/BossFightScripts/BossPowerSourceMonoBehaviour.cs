using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPowerSourceMonoBehaviour : MonoBehaviour
{


    public GymBossMonoBehaviour boss;
    public float respawnTime = 60.0f;
    protected Health health;
    
    protected void Start()
    {
        health = GetComponent<Health>();
        boss.powerSources += 1;
        UpdateBossVincibility();
    }

    public void Dormant()
    {
        boss.powerSources -= 1;
        UpdateBossVincibility();
        StartCoroutine(Respawn());
    }

    protected IEnumerator Respawn()
    {
        health.invincible = true;
        float respawnCountdown = respawnTime;
        while (respawnCountdown > 0)
        {
            respawnCountdown -= Time.deltaTime;
            yield return null;
        }

        boss.powerSources += 1;
        UpdateBossVincibility();
        health.health = health.maxHealth;
        health.invincible = false;
    }

    protected void UpdateBossVincibility()
    {
        if (boss.powerSources > 0)
        {
            boss.health.invincible = true;
        }
        else
        {
            boss.health.invincible = false;
        }
    }
}
