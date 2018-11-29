﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;

public class PlayerWaterControl : MonoBehaviour {
	Rigidbody2D rb;
	SpriteRenderer spriteRenderer;

	PlayerMovement playerMovement;

	public float swimDashImpulse;
	public float maxSpeed;

	public AnimationCurve swimMovementForceCurve;
	public float maxSwimForce = 30;
	public float maxSwimSpeed = 30;

	public float waterDragNoInput = .5f;
	public float waterDragInput = .1f;

	public GameObject projectilePrefab;
	public float projectileOffset = 1;
	public float projectileFireRate = 10;

	private float _shotCooldown = 0;

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
		Vector2 movement = playerMovement.fishPlayer.GetAxis2D("AimHorizontal", "AimVertical");
		if (movement.sqrMagnitude != 0) {
			playerMovement.aimDirection = movement.normalized;
		}

		//if (playerMovement.fishPlayer.GetButton("SecondaryAbility")) {
		//	if (_shotCooldown <= 0) {
		//		Projectile shootyshoot = ObjectPoolManager.GetObject(projectilePrefab).GetComponent<Projectile>();
		//		shootyshoot.transform.position = transform.position + (Vector3)playerMovement.aimDirection * projectileOffset;
		//		shootyshoot.direction = playerMovement.aimDirection;

		//		shootyshoot.SetSpeed(Vector2.Dot(rb.velocity, shootyshoot.direction));
		//		shootyshoot.transform.right = playerMovement.aimDirection;

		//		_shotCooldown = 1 / projectileFireRate;
		//	}
			
		//}
		//_shotCooldown -= Time.deltaTime;
	}

	void UpdateSwim() {
		bool moveAbilityDown = playerMovement.fishPlayer.GetButtonDown("Move Ability");
		if (moveAbilityDown) {
			rb.AddCappedForce(rb.velocity.normalized * swimDashImpulse, maxSpeed, ForceMode2D.Impulse);
		}
	}

	void FixedUpdateFly() {

	}

	void FixedUpdateSwim() {
		Vector2 movement = playerMovement.fishPlayer.GetAxis2D("Horizontal", "Vertical");
		if (movement.magnitude > 1) movement.Normalize();

		Vector2 targetSpeed = movement * maxSwimSpeed;
		rb.AddForceToAchieveTargetVelocity(targetSpeed, maxSwimForce, swimMovementForceCurve);

		if (movement.magnitude > 0) {
			rb.drag = waterDragInput;
		}
		else {
			rb.drag = waterDragNoInput;
		}
	}
}
