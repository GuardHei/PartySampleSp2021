using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public bool inControl = true;
	public float speed = 5f;
	
	public KeyCode upKey = KeyCode.W;
	public KeyCode downKey = KeyCode.S;
	public KeyCode leftKey = KeyCode.A;
	public KeyCode rightKey = KeyCode.D;
	public KeyCode attackKey = KeyCode.J;
	public KeyCode itemKey = KeyCode.K;

	public Rigidbody2D rigidbody2D;

	public Vector2 velocity;

	public void Update() {
		velocity = Vector2.zero;
		
		if (inControl) {
			float vertical = (Input.GetKey(upKey) ? 1 : 0) + (Input.GetKey(downKey) ? -1 : 0);
			float horizontal = (Input.GetKey(rightKey) ? 1 : 0) + (Input.GetKey(leftKey) ? -1 : 0);
			velocity = new Vector2(horizontal, vertical).normalized * speed;
		}

		rigidbody2D.velocity = velocity;
	}
}