using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public enum PlayerMovementState {
	SWIMMING,
	FLYING
};

public class PlayerMovement : MonoBehaviour {

	Rigidbody2D rb;
	SpriteRenderer spriteRenderer;

	public PlayerMovementState state;

	public Vector2 initialImpulse;
	public float maxSpeed;
	public Transform aimerPivotTransform;
	public Transform projectileSpawnPoint;

	public Player fishPlayer;
	public Player birdPlayer;

	public Vector2 aimDirection;

	public bool singlePlayer = false;
	public float enterWaterImpulse = 5;
	public float exitWaterImpulse = 5;

	void Start() {
		fishPlayer = ReInput.players.GetPlayer(GameManager.instance.FishPlayerID);
		birdPlayer = ReInput.players.GetPlayer(GameManager.instance.BirdPlayerID);
		if(GameManager.instance.FishPlayerID == GameManager.instance.BirdPlayerID) {
			singlePlayer = true;
		}
		
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
 		spriteRenderer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		spriteRenderer.flipY = rb.velocity.x < 0;

		aimerPivotTransform.right = aimDirection.normalized;
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
			rb.AddForce(rb.velocity.normalized * enterWaterImpulse, ForceMode2D.Impulse);
			//StartCoroutine(SlowTime(.5f, .1f));
		}
     }
     
     void OnTriggerExit2D(Collider2D other) {
         if (other.tag == "Water") {
             state = PlayerMovementState.FLYING;
			rb.AddForce(rb.velocity.normalized * exitWaterImpulse, ForceMode2D.Impulse);
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
