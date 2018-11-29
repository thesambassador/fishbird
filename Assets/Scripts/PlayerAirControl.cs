using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;

public class PlayerAirControl : MonoBehaviour {
	Player player;
	Rigidbody2D rb;
	SpriteRenderer spriteRenderer;

	PlayerMovement playerMovement;

	public int playerId;
	public bool flapEnabled;
	public float flapImpulse;

	public float maxSpeed;
	public float liftForce;
	public float maxAirDrag = .3f;
	public float pullUpDrag = .5f;

	public AnimationCurve flapImpulseCurve;

	public GameObject projectilePrefab;
	public float projectileOffset = 1.5f;

	void Awake() {
		player = ReInput.players.GetPlayer(playerId);
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
		bool moveAbilityDown = player.GetButtonDown("Move Ability");
		if (moveAbilityDown && flapEnabled) {
			Flap();
		}
	}

	void Flap() {
		float flapAmount = flapImpulse * flapImpulseCurve.Evaluate(Mathf.Max(0, rb.velocity.y) / maxSpeed);
		rb.AddCappedForce(Vector2.up * flapAmount, maxSpeed, ForceMode2D.Impulse);
	}

	void UpdateSwim() {
		Vector2 movement = player.GetAxis2D("AimHorizontal", "AimVertical");
		if (movement.sqrMagnitude != 0) {
			playerMovement.aimDirection = movement.normalized;
		}

		if(player.GetButtonDown("SecondaryAbility")){
			Projectile shootyshoot = ObjectPoolManager.GetObject(projectilePrefab).GetComponent<Projectile>();
			shootyshoot.transform.position = transform.position + (Vector3)playerMovement.aimDirection * projectileOffset;
			shootyshoot.direction = playerMovement.aimDirection;

			shootyshoot.SetSpeed(Vector2.Dot(rb.velocity, shootyshoot.direction));

			shootyshoot.transform.right = playerMovement.aimDirection;
		}
	}

	void FixedUpdateFly() {
		float pitch = player.GetAxis("Pitch");

		Vector2 pitchForce = Vector2.Perpendicular(rb.velocity) * pitch * liftForce;

		rb.AddForce(pitchForce);

		//rb.AddCappedForce(pitchForce, maxSpeed);
		//if (pitchForce.magnitude > 0) {
		//	rb.AddCappedForce(rb.velocity * 0.1f, maxSpeed);
		//}

		float dot = Mathf.Max(0, Vector2.Dot(rb.velocity.normalized, Vector2.up));
		float drag = maxAirDrag * dot;

		if(pitchForce.y > 0) {
			drag += pullUpDrag;
		}

		rb.drag = drag;

		Debug.DrawLine(transform.position, transform.position + (Vector3)(pitchForce * 0.1f), Color.red);
	}

	void FixedUpdateSwim() {

	}
}
