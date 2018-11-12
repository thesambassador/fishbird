using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public enum PlayerMovementState {
	SWIMMING,
	FLYING
};

public class PlayerMovement : MonoBehaviour {

	public int playerId;
	Player player;
	Rigidbody2D rb;
	SpriteRenderer spriteRenderer;

	public PlayerMovementState state;

	public bool flapEnabled;
	public float flapImpulse;

	public float liftForce;

	public Vector2 initialImpulse;
	public float maxSpeed;
	public float airMovementSpeed;

	public AnimationCurve flapImpulseCurve;

	public AnimationCurve swimMovementForceCurve;
	public float maxSwimForce = 30;
	public float maxSwimSpeed = 30;

	public float swimDashImpulse;

	public float maxAirDrag = .3f;
	public float waterDragNoInput = .5f;
	public float waterDragInput = .1f;

	void Awake() {
		player = ReInput.players.GetPlayer(playerId);
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Start() {
		rb.AddCappedForce(initialImpulse, maxSpeed, ForceMode2D.Impulse);
	}

	void Update () {
		switch (state) {
			case PlayerMovementState.FLYING:
				UpdateFly();
				break;
			case PlayerMovementState.SWIMMING:
			default:
				UpdateSwim();
				break;
		}

		float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
 		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		spriteRenderer.flipY = rb.velocity.x < 0;
	}

	void FixedUpdate() {
		switch (state) {
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

	void UpdateSwim() {
		bool moveAbilityDown = player.GetButtonDown("Move Ability");
		if (moveAbilityDown) {
			rb.AddCappedForce(rb.velocity.normalized * swimDashImpulse, maxSpeed, ForceMode2D.Impulse);
		}
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
		Vector2 movement = player.GetAxis2D("Horizontal", "Vertical");
		if (movement.magnitude > 1) movement.Normalize();

		Vector2 targetSpeed = movement * maxSwimSpeed;

		Vector2 curTargetDiff = targetSpeed - rb.velocity;

		Debug.DrawLine(transform.position, transform.position + (Vector3)(curTargetDiff), Color.green);

		//avoid divide by 0 i guess
		float forceToUse = 0;
		print(Vector2.Dot(targetSpeed, curTargetDiff));
		if (targetSpeed.magnitude > 0 && Vector2.Dot(targetSpeed, curTargetDiff) >= 0) {
			// if curTargetDiff.magnitude is 0, that means that we're moving at the target speed already
			forceToUse = swimMovementForceCurve.Evaluate(curTargetDiff.magnitude / targetSpeed.magnitude) * maxSwimForce;
		}
	

		if(movement.magnitude > 0) {
			rb.drag = waterDragInput;
		}
		else {
			rb.drag = waterDragNoInput;
		}

		Debug.DrawLine(transform.position, transform.position + (Vector3)(forceToUse * movement * 0.1f), Color.red);

		rb.AddForce(forceToUse * curTargetDiff.normalized);
		print("Current vel magnitude: " + rb.velocity.magnitude + "force used: " + forceToUse);
	}

	void Flap() {
		float flapAmount = flapImpulse * flapImpulseCurve.Evaluate(Mathf.Max(0, rb.velocity.y) / maxSpeed);
		rb.AddCappedForce(Vector2.up * flapAmount, maxSpeed, ForceMode2D.Impulse);
	}

	void OnTriggerEnter2D(Collider2D other) {
         if (other.tag == "Water") {
             state = PlayerMovementState.SWIMMING;
			//StartCoroutine(SlowTime(.5f, .1f));
		}
     }
     
     void OnTriggerExit2D(Collider2D other) {
         if (other.tag == "Water") {
             state = PlayerMovementState.FLYING;
			//StartCoroutine(SlowTime(.25f, .1f));
         }
     }

	IEnumerator SlowTime(float time, float newTimescale) {
		Time.timeScale = newTimescale;

		float originalFixed = Time.fixedDeltaTime;
		Time.fixedDeltaTime = Time.fixedDeltaTime * newTimescale;

		yield return new WaitForSeconds(time * newTimescale);

		Time.timeScale = 1;
		Time.fixedDeltaTime = originalFixed;

	}

}
