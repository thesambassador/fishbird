using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Basic bird enemy
 * Better at attacking stuff below it than above it
 * if target is above, needs to flap to get high
 * if target is below, can dive towards them with increasing speed
 */

public class AirEnemyBehavior : MonoBehaviour {
	public enum AirEnemyState {
		Idle,
		ChaseDive,
		ChaseAscend
	}

	public Transform target;
	public PlayerMovement pMovement;
	public AirEnemyState state;

	private Rigidbody2D _rb;

	public float diveDrag = 0;
	public float slowdownDrag = 10;
	public float normalDrag = 0.3f;

	public float normalDragVelocity = 5;

	public Vector2 desiredForce;

	public float maxSpeed = 30;
	public float diveForce = 30;
	public float ascendHorizontalForce = 10;

	public float flapImpulse = 10;
	public float flapCooldown = .75f;
	public float flapTriggerVel = -1;
	public float horizontalZoneSize = 5;

	private bool _canFlap = true;

	public Vector2 startingPosition;
	public Vector2 desiredPosition;

	// Use this for initialization
	void Awake () {
		_rb = GetComponent<Rigidbody2D>();
		startingPosition = transform.position;
		
	}

	private void OnEnable() {
		_rb.velocity = new Vector2();
		_rb.angularVelocity = 0;
		state = AirEnemyState.Idle;
	}

	// Update is called once per frame
	void Update () {
		state = DetermineState();

		switch (state) {
			case AirEnemyState.Idle:
				UpdateIdle();
				break;
			case AirEnemyState.ChaseDive:
				UpdateChaseDive();
				break;
			case AirEnemyState.ChaseAscend:
				UpdateChaseAscend();
				break;
		}
	}

	void FixedUpdate() {
		_rb.AddCappedForce(desiredForce, maxSpeed, ForceMode2D.Force);
	}

	void UpdateIdle() {
		_rb.gravityScale = 1;
		desiredForce.x = 0;
		desiredForce.y = 0;

		desiredPosition = startingPosition;

		SlowDown();

		Flap();
	}

	void UpdateChaseDive() {
		_rb.drag = diveDrag;
		_rb.gravityScale = 0;

		Vector2 targetVector = target.position - transform.position;
		desiredForce = targetVector.normalized * diveForce;
	}


	void UpdateChaseAscend() {

		_rb.gravityScale = 1;

		Vector2 targetVector = target.position - transform.position;

		desiredForce.y = 0;
		desiredForce.x = Mathf.Sign(targetVector.x) * ascendHorizontalForce;
		desiredPosition.y = target.position.y + 5;
		desiredPosition.x = target.position.x;

		SlowDown();

		Flap();
	}

	void SlowDown() {
		if (Mathf.Abs(desiredPosition.x - transform.position.x) < horizontalZoneSize && _rb.velocity.magnitude > normalDragVelocity) {
			_rb.drag = slowdownDrag;
		}
		else {
			_rb.drag = normalDrag;
		}
	}

	void Flap() {
		if (_canFlap) {
			if(_rb.velocity.y < flapTriggerVel && transform.position.y < desiredPosition.y) {
				_rb.AddForce(new Vector2(0, flapImpulse), ForceMode2D.Impulse);
				StartCoroutine(FlapCooldown());
			}
		}
	}

	IEnumerator FlapCooldown() {
		_canFlap = false;
		yield return new WaitForSeconds(flapCooldown);
		_canFlap = true;
	}

	AirEnemyState DetermineState() {
		if(target == null) {
			return AirEnemyState.Idle;
		}
		else if(pMovement.state == PlayerMovementState.SWIMMING) {
			return AirEnemyState.Idle;
		}
		else if(target.position.y < transform.position.y) {
			return AirEnemyState.ChaseDive;
		}
		else {
			return AirEnemyState.ChaseAscend;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			target = collision.transform;
			pMovement = collision.GetComponent<PlayerMovement>();
		}
	}
}
