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

	public AnimationCurve flapImpulseCurve;

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

	}

	void FixedUpdateFly() {
		float pitch = player.GetAxis("Pitch");

		Vector2 pitchForce = Vector2.Perpendicular(rb.velocity) * pitch * liftForce;

		rb.AddCappedForce(pitchForce, maxSpeed);
		if (pitchForce.magnitude > 0) {
			rb.AddCappedForce(rb.velocity * 0.1f, maxSpeed);
		}

		float dot = Mathf.Abs(Vector2.Dot(rb.velocity.normalized, Vector2.down));
		rb.drag = maxAirDrag * dot;

		Debug.DrawLine(transform.position, transform.position + (Vector3)(pitchForce * 0.1f), Color.red);
	}

	void FixedUpdateSwim() {

	}
}
