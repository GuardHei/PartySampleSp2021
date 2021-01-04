using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HitboxController : MonoBehaviour {

	public int damage;
	public string[] damageTags;
	
	public void OnTriggerEnter2D(Collider2D other) {
		if (damageTags != null) {
			GameObject go = other.gameObject;
			foreach (var tag in damageTags) {
				if (go.CompareTag(tag)) {
					if (TryGetComponent(out Health health)) health.Hit(damage);
					return;
				}
			}
		}
	}
}