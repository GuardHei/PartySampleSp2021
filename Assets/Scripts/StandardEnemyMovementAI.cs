using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StandardEnemyMovementAI : MonoBehaviour {

	public float speed;
	public float stoppingDistance;
	public Transform currentTarget;
	
	public bool isPatrolling = true;
	
	public bool inPath;
	public List<PathPoint> pathPoints = new List<PathPoint>();
	public int currentPathPoint;

	public Sprite upSprite;
	public Sprite downSprite;
	public Sprite leftSprite;
	public Sprite rightSprite;
	
	public Rigidbody2D rigidbody2D;
	public Transform face;
	public Direction facing;
	public Transform up;
	public Transform down;
	public Transform left;
	public Transform right;
	public Vector2 velocity;

	[Conditional("UNITY_EDITOR")]
	public void OnDrawGizmosSelected() {
		if (pathPoints != null) {
			for (int i = 0; i < pathPoints.Count; i++) {
				Gizmos.color = i == 0 ? Color.yellow : Color.green;
				if (pathPoints[i] == null || pathPoints[i].target == null) return;
				var pos = pathPoints[i].target.position;
				Gizmos.DrawCube(pos, .5f * Vector3.one);
				var from = i == 0 ? pathPoints.Count - 1 : i - 1;
				if (pathPoints[from] == null || pathPoints[from].target == null) return;
				Gizmos.color = Color.green;
				Gizmos.DrawLine(pathPoints[from].target.position, pos);
			}
		}
	}

	public void Awake() {
		currentTarget = GameObject.FindWithTag("Player").transform;
		if (pathPoints != null && pathPoints.Count > 0) currentPathPoint = 0;
		Turn(Direction.DOWN);
	}

	public void FixedUpdate() {
		if (!isPatrolling) MoveTowardsPlayer();
		else MoveTowardsPathPoint();
		rigidbody2D.velocity = velocity;
	}

	public void MoveTowardsPlayer() {
		if (currentTarget == null) return;
		var distance = currentTarget.position - transform.position;
		var mag = distance.magnitude;
		if (mag <= stoppingDistance) {
			velocity = Vector2.zero;
			return;
		}
		
		var dir = ChooseDirection(distance);
		Turn(dir);
		velocity = speed * distance / mag;
	}

	public void MoveTowardsPathPoint() {
		if (currentPathPoint < 0 || currentPathPoint >= pathPoints.Count) return;
		var pathPoint = pathPoints?[currentPathPoint];
		if (pathPoint == null) return;
		var distance = pathPoint.target.position - transform.position;
		if (ReachCurrentPathPoint(distance)) {
			transform.position = pathPoint.target.position;
			inPath = true;
			Turn(pathPoint.facing);
			currentPathPoint++;
			if (currentPathPoint == pathPoints.Count) currentPathPoint = 0;
			pathPoint = pathPoints[currentPathPoint];
			pathPoint.speed = (pathPoint.target.position - transform.position).magnitude / pathPoint.time;
		}
		
		velocity = (inPath ? pathPoint.speed : speed) * distance.normalized;
		var dir = ChooseDirection(velocity);
		Turn(dir);
	}

	public Direction ChooseDirection(Vector2 distance) {
		if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y)) return distance.x < 0 ? Direction.LEFT : Direction.RIGHT;
		return distance.y < 0 ? Direction.DOWN : Direction.UP;
	}

	public void Turn(Direction direction) {
		if (direction == facing) return;
		facing = direction;
		if (face != null) {
			Vector3 localPos = Vector3.zero;
			Vector3 eulerAngle = Vector3.one;
			switch (direction) {
				case Direction.UP:
					if (up != null) localPos = up.localPosition;
					eulerAngle = new Vector3(0, 0, 0);
					GetComponent<SpriteRenderer>().sprite = upSprite;
					break;
				case Direction.DOWN:
					if (down != null) localPos = down.localPosition;
					eulerAngle = new Vector3(0, 0, 180);
					GetComponent<SpriteRenderer>().sprite = downSprite;
					break;
				case Direction.LEFT:
					if (left != null) localPos = left.localPosition;
					eulerAngle = new Vector3(0, 0, 90);
					GetComponent<SpriteRenderer>().sprite = leftSprite;
					break;
				case Direction.RIGHT:
					if (right != null) localPos = right.localPosition;
					eulerAngle = new Vector3(0, 0, -90);
					GetComponent<SpriteRenderer>().sprite = rightSprite;
					break;
			}

			face.localPosition = localPos;
			face.rotation = Quaternion.Euler(eulerAngle);
		}
	}

	public void TargetPlayer() {
		isPatrolling = false;
		inPath = false;
	}

	public void TargetPathPoint() => isPatrolling = true;

	public bool ReachCurrentPathPoint(Vector2 distance, float threshold = .05f) {
		if (pathPoints[currentPathPoint] == null) return false;
		return distance.magnitude <= threshold;
	}
}

[Serializable]
public class PathPoint {
	public Transform target;
	public Direction facing;
	public float time;
	public float speed;
}