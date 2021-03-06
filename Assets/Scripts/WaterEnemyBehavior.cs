﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState {
	Idle,
	Chasing
}

public class WaterEnemyBehavior : MonoBehaviour {

	public EnemyState state;

	public Transform targetTransform;

	public float killVelocity;
	public float strokeCooldown;
	public Animator animator;
	public Rigidbody2D rb;

	public float StrokeImpulse = 10;

	private bool _pointAtPlayer = false;
	private float _strokeCooldownTimer = 0;

	void OnEnable() {
		state = EnemyState.Idle;
		rb.velocity = new Vector2();
		rb.angularVelocity = 0;
		animator.ResetTrigger("SwimStroke");
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
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
		if(targetTransform == null) {
			state = EnemyState.Idle;
			return;
		}
		
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
			_pointAtPlayer = true;
		}

		if (_pointAtPlayer) {
			PointAtPlayer();
		}

		MoveTowardsPlayer();
	}

	void PointAtPlayer() {
		Vector2 toTarget = targetTransform.position - transform.position;
		float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
		Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward);

		transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, .3f);

		//transform.right = toTarget.normalized;
	}

	void MoveTowardsPlayer() {
		_strokeCooldownTimer -= Time.deltaTime;
		if(_strokeCooldownTimer <= 0) {
			animator.SetTrigger("SwimStroke");
			_strokeCooldownTimer = strokeCooldown;
			
		}
	}

	//for potential pathfinding in the future
	Vector2 DetermineDirection() {
		return (targetTransform.position - transform.position).normalized;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			targetTransform = collision.gameObject.transform;
			state = EnemyState.Chasing;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag == "Ground") {
			//print(collision.relativeVelocity.magnitude);
			if(collision.relativeVelocity.magnitude > killVelocity) {
				ObjectPoolManager.ReturnObject(this.gameObject);
			}
		}
	}

	void SwimImpulse() {
		_pointAtPlayer = false;
		rb.AddForce(transform.right * StrokeImpulse, ForceMode2D.Impulse);
	}
}
