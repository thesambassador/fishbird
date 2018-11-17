using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState {
	Idle,
	Chasing
}

public class WaterEnemyBehavior : MonoBehaviour {

	public EnemyState State;

	public Transform TargetTransform;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		switch (State) {
			case EnemyState.Idle:
				IdleState();
				break;
			case EnemyState.Chasing:
				ChaseState();
				break;
		}
	}

	void IdleState() {

	}

	void ChaseState() {
		PointAtPlayer();
		MoveTowardsPlayer();
	}

	void PointAtPlayer() {
		Vector2 toTarget = TargetTransform.position - transform.position;
		transform.right = toTarget.normalized;
	}

	void MoveTowardsPlayer() {

	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			State = EnemyState.Chasing;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		print("ahhhh");
	}
}
