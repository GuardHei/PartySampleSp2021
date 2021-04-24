using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {
	
	public bool invincible;
	public bool armored;

	public int maxHealth;
	public int health;

	public UnityEvent onHit;
	public UnityEvent onRecover;
	public UnityEvent onDeath;

	private void Awake() => health = maxHealth;

	public void Hit(int damage, bool ignoreArmor = false) {
		if (invincible) return;
		if (armored && !ignoreArmor) return;
		health -= damage;
		health = Mathf.Max(health, 0);
		onHit?.Invoke();
		print(name + " takes " + damage + " dmg");
		if (health == 0) Die();
	}

	public void Recover(int cure) {
		health += cure;
		health = Mathf.Min(health, maxHealth);
		onRecover?.Invoke();
	}

	public void Die() {
		onDeath?.Invoke();
	}

	public void SelfDestroy() => Destroy(gameObject);
}
