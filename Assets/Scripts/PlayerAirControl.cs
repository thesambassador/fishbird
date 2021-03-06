﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;

public class PlayerAirControl : MonoBehaviour {
	Rigidbody2D rb;
	SpriteRenderer spriteRenderer;

	PlayerMovement playerMovement;

	public bool flapEnabled;
	public float flapImpulse;

	public float maxSpeed;
	public float liftForce;
	public float maxAirDrag = .3f;
	public float minAirDrag = .1f;
	public float pullUpDrag = .5f;

	public AnimationCurve flapImpulseCurve;

	public GameObject projectilePrefab;
	public float projectileOffset = 1.5f;

	void Awake() {
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		playerMovement = GetComponent<PlayerMovement>();
	}

	void Start() {
		
	}

	void Update() {
		switch (playerMovement.state) {
			case PlayerMovementState.FLYING:
				UpdateFly();
				break;
			case PlayerMovementState.SWIMMING:
			default:
				UpdateSwim();
				break;
		}
	}

	void FixedUpdate() {
		switch (playerMovement.state) {
			case PlayerMovementState.FLYING:
				FixedUpdateFly();
				break;
			case PlayerMovementState.SWIMMING:
			default:
				FixedUpdateSwim();
				break;
		}
	}

	void UpdateFly() {

	}

	void Flap() {
		float flapAmount = flapImpulse * flapImpulseCurve.Evaluate(Mathf.Max(0, rb.velocity.y) / maxSpeed);
		rb.AddCappedForce(Vector2.up * flapAmount, maxSpeed, ForceMode2D.Impulse);
	}

	void UpdateSwim() {
		//Vector2 aim = playerMovement.birdPlayer.GetAxis2D("Horizontal", "Vertical");
		//if (playerMovement.singlePlayer) {
		//	aim = rb.velocity.normalized;
		//}
		//if (aim.sqrMagnitude != 0) {
		//	playerMovement.aimDirection = aim.normalized;
		//}

		//if(playerMovement.birdPlayer.GetButtonDown("SecondaryAbility")){
		//	Projectile shootyshoot = ObjectPoolManager.GetObject(projectilePrefab).GetComponent<Projectile>();
		//	shootyshoot.transform.position = transform.position + (Vector3)playerMovement.aimDirection * projectileOffset;
		//	shootyshoot.direction = playerMovement.aimDirection;

		//	shootyshoot.SetSpeed(Vector2.Dot(rb.velocity, shootyshoot.direction));

		//	shootyshoot.transform.right = playerMovement.aimDirection;
		//}
	}

	void FixedUpdateFly() {
		float pitch = playerMovement.birdPlayer.GetAxis("Pitch");
		if (playerMovement.moveInputDisabled)
			pitch = 0;

		Vector2 pitchForce = Vector2.Perpendicular(rb.velocity) * pitch * liftForce;

		rb.AddForce(pitchForce);

		//rb.AddCappedForce(pitchForce, maxSpeed);
		//if (pitchForce.magnitude > 0) {
		//	rb.AddCappedForce(rb.velocity * 0.1f, maxSpeed);
		//}

		float dot = Mathf.Max(0, Vector2.Dot(rb.velocity.normalized, Vector2.up));
		float drag = Mathf.Max(maxAirDrag * dot, minAirDrag);

		if(pitchForce.y > 0) {
			drag += pullUpDrag;
		}

		rb.drag = drag;

		Debug.DrawLine(transform.position, transform.position + (Vector3)(pitchForce * 0.1f), Color.red);
	}

	void FixedUpdateSwim() {

	}
}
