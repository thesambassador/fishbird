using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;

public class AlternativeAirMovement : MonoBehaviour {
	Player player;
	Rigidbody2D rb;
	SpriteRenderer spriteRenderer;

	PlayerMovement playerMovement;
	public int playerId;

	public float yVelocityStableRange = 1; //plus or minus this amount is the y velocity we want when not diving
	public float liftForce = 9.5f; //add this much force as lift
	public AnimationCurve xVelLiftForceCurve; //add this percentage of the liftforce based on horizontal velocity
	public float xVelForLift; //at this velocity or higher, we apply max lift force

	public float stabilizationLiftForce = 20f; //add this much force to try and get back to the stable range

	public float glideHorizontalSpeed = 20f;
	public float glideHorizontalForce = 30f;

	public float diveImpulse = 10;
	public float diveForce = 10;
	public AnimationCurve diveVelocityForceCurve;

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

	void UpdateFly() {
		bool moveAbilityDown = player.GetButtonDown("Move Ability");
		Vector2 movement = player.GetAxis2D("Horizontal", "Vertical");

		if (moveAbilityDown) {
			//start dive
			Vector2 diveDirection = ClampVectorToAngleFromVector(movement.normalized, Vector2.down, 45);
			rb.AddForce(diveDirection * diveImpulse, ForceMode2D.Impulse);
		}


	}

	Vector2 ClampVectorToAngleFromVector(Vector2 input, Vector2 refVector, float angleDegrees) {
		float angle = Vector2.Angle(refVector, input);
		if(angle > angleDegrees) {
			return Vector3.RotateTowards(refVector, input, angleDegrees * Mathf.Deg2Rad, Mathf.PI);
		}
		else {
			return input;
		}
		
	}


	void UpdateSwim() {

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

	void FixedUpdateFly() {
		bool moveAbilityHeld = player.GetButton("Move Ability");
		Vector2 movement = player.GetAxis2D("Horizontal", "Vertical");

		//diving
		if (moveAbilityHeld) {
			Vector2 diveDirection = ClampVectorToAngleFromVector(movement.normalized, Vector2.down, 90);
			rb.AddForceToAchieveTargetVelocity(diveDirection * rb.velocity.magnitude, diveForce, diveVelocityForceCurve);

			Debug.DrawLine(transform.position, transform.position + (Vector3)diveDirection * 5);
		}
		//normal gliding
		else {
			//lift factor
			float liftFactor = xVelLiftForceCurve.Evaluate(Mathf.Abs(rb.velocity.x) / xVelForLift); 
			//normal lift
			if (rb.velocity.y < yVelocityStableRange) {
				float liftToUse = liftForce * liftFactor;
				rb.AddForce(Vector2.up * liftToUse);
				print(liftToUse);
			}
			//if we aren't trying to dive, but we're going down fast still, stabilize! 
			if(rb.velocity.y < -yVelocityStableRange) {
				rb.AddForce(Vector2.up * stabilizationLiftForce * liftFactor);
			}

			//now left/right movement
			if (Mathf.Abs(rb.velocity.x) < glideHorizontalSpeed) {
				rb.AddForce(movement.x * Vector2.right * glideHorizontalForce);
			}

		}


	}

	void FixedUpdateSwim() {

	}
}
