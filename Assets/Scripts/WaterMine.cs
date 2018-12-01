using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMine : MonoBehaviour {
	PlayerMovement _player;
	public bool active = false;
	public Rigidbody2D rb;

	public float targetDistFromPlayer = 1.2f;
	public AnimationCurve velocityForceCurve;
	public float maxForce;
	public float maxSpeed;

	public float explodeTime;
	public AnimationCurve flashFrequencyCurve;
	public GameObject explosionPrefab;
	public FlashSprite flashSprite;

	private float _explodeTimer = 3;
	private float _flashTimer;

	public bool inWater = false;

	// Use this for initialization
	void OnEnable () {
		active = false;
	}

	void Update() {
		if (active) {
			if (_explodeTimer > 0) {
				_explodeTimer -= Time.deltaTime;
			}
			else {
				Explode();
			}

			if (_flashTimer > 0) {
				_flashTimer -= Time.deltaTime;
			}
			else {
				flashSprite.Flash();
				_flashTimer = flashFrequencyCurve.Evaluate(_explodeTimer);
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (active) {
			if (_player != null) {
				Vector2 toPlayer = _player.transform.position - transform.position;
				float dist = toPlayer.magnitude;

				Vector2 targetSpeed = toPlayer.normalized * maxSpeed;
				if (dist > targetDistFromPlayer && inWater && _player.state == PlayerMovementState.SWIMMING) {
					rb.AddForceToAchieveTargetVelocity(targetSpeed, maxForce, velocityForceCurve);
					rb.drag = .5f;
				}
				else {
					rb.drag = 5;
				}
			}
		}
	}

	void Explode() {
		GameObject explosion = ObjectPoolManager.GetObject(explosionPrefab, transform.position, Quaternion.identity);
		ObjectPoolManager.ReturnObject(explosion, 1);
		ObjectPoolManager.ReturnObject(this.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == "Player" && !active) {
			_player = collision.GetComponent<PlayerMovement>();
			active = true;
			_explodeTimer = explodeTime;
			_flashTimer = 1;
		}
		else if(collision.tag == "Water") {
			inWater = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.tag == "Water") {
			inWater = false;
		}
	}
		
	private void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag == "Terrain" && collision.contacts[0].normalImpulse > 2) {
			Explode();
		}
	}
}
