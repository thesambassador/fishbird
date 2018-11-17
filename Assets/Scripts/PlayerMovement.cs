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

	public Vector2 initialImpulse;
	public float maxSpeed;

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

	}

	void UpdateSwim() {

	}

	void FixedUpdateFly() {

	}

	void FixedUpdateSwim() {


	}

	void Flap() {

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
