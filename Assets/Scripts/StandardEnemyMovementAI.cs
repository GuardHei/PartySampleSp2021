using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnemyMovementAI : MonoBehaviour {

	public bool isPatrolling = true;
	
	public bool loop = true;
	public List<PathPoint> pathPoints = new List<PathPoint>();
	public PathPoint currentPathPoint;
	
	public Rigidbody2D rigidbody2D;
	public Direction facing;
	public Vector2 velocity;
	public Transform currentTarget;

	public void Awake() {
		
	}

	public void Update() {
		
	}
}

[Serializable]
public class PathPoint {
	public Transform position;
	public float rotation;
	public float speed;
	public float angularSpeed;
}