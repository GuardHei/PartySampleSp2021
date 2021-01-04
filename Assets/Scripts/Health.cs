using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

	public int maxHealth;
	public int health;

	public UnityEvent onHit;
	public UnityEvent onRecover;
	public UnityEvent onDeath;

	public void Hit(int damage) {
		health -= damage;
		health = Mathf.Max(health, 0);
		onHit?.Invoke();
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
}
