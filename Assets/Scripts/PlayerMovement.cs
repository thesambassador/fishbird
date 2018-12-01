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
	public SpriteRenderer aimerSprite;
	public Transform projectileSpawnPoint;

	public Player fishPlayer;
	public Player birdPlayer;

	public Vector2 aimDirection;

	public bool singlePlayer = false;
	public float enterWaterImpulse = 5;
	public float exitWaterImpulse = 5;

	public float waterTransitionInputDisableTime = .2f;

	public bool moveInputDisabled = false;

	private float _slowDeathTimer = 3;
	public float SlowDeathTime = 3;
	public float SlowDeathVelocityThreshold = 3;

	public Health playerHealth;

	void Start() {
		fishPlayer = ReInput.players.GetPlayer(GameManager.instance.FishPlayerID);
		birdPlayer = ReInput.players.GetPlayer(GameManager.instance.BirdPlayerID);
		if (GameManager.instance.FishPlayerID == GameManager.instance.BirdPlayerID) {
			singlePlayer = true;
		}

		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		rb.AddCappedForce(initialImpulse, maxSpeed, ForceMode2D.Impulse);

		_slowDeathTimer = SlowDeathTime;
	}

	void Update() {
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
		_timeSinceTerrainCollision += Time.deltaTime;

		if (rb.velocity.magnitude < SlowDeathVelocityThreshold) {
			_slowDeathTimer -= Time.deltaTime;
			if(_slowDeathTimer <= 0 && _timeSinceTerrainCollision < 1) {
				playerHealth.Kill();
			}
		}
		else {
			_slowDeathTimer = SlowDeathTime;
		}
	}

	void UpdateSwim() {

	}

	void FixedUpdateFly() {

	}

	void FixedUpdateSwim() {


	}

	void Flap() {

	}

	public void SetAimerVisibility(bool visible) {
		//aimer always visible underwater
		if (state == PlayerMovementState.SWIMMING) {
			aimerSprite.enabled = true;
		}
		else {
			aimerSprite.enabled = visible;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Water") {
			state = PlayerMovementState.SWIMMING;
			rb.AddForce(rb.velocity.normalized * enterWaterImpulse, ForceMode2D.Impulse);
			StartCoroutine(DisableMovementInput(.1f));
			SetAimerVisibility(true);
			//StartCoroutine(SlowTime(.5f, .1f));
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Water") { 
			state = PlayerMovementState.FLYING;
			SetAimerVisibility(false);
			rb.AddForce(rb.velocity.normalized * exitWaterImpulse, ForceMode2D.Impulse);
			TempDisableMovement(waterTransitionInputDisableTime);
			//StartCoroutine(SlowTime(.25f, .1f));
		}
	}

	public void TempDisableMovement(float time) {
		StartCoroutine(DisableMovementInput(time));
	}

	IEnumerator DisableMovementInput(float time) {
		moveInputDisabled = true;
		yield return new WaitForSeconds(time);
		moveInputDisabled = false;
	}

	IEnumerator SlowTime(float time, float newTimescale) {
		Time.timeScale = newTimescale;

		float originalFixed = Time.fixedDeltaTime;
		Time.fixedDeltaTime = Time.fixedDeltaTime * newTimescale;

		yield return new WaitForSeconds(time * newTimescale);

		Time.timeScale = 1;
		Time.fixedDeltaTime = originalFixed;

	}

	private float _timeSinceTerrainCollision = 500;
	private void OnCollisionStay2D(Collision2D collision) {
		if(collision.gameObject.tag == "Terrain") {
			_timeSinceTerrainCollision = 0;
		}
	}

}
