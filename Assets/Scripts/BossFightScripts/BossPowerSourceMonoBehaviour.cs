using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPowerSourceMonoBehaviour : MonoBehaviour
{


    public Sprite respawnSprite;
    public GymBossMonoBehaviour boss;
    public float respawnTime = 120.0f;
    public DialogueSettings onRespawnDialogue;
    protected Health health;
    protected SpriteRenderer spriteRenderer;

    protected void Start()
    {
        health = GetComponent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        spriteRenderer.sprite = respawnSprite;
        DialogueManager.Instance.Open(onRespawnDialogue);
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
