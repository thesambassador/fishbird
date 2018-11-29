using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlingForceComponent : MonoBehaviour {
	private float _originalGravityScale;
	private Rigidbody2D _rb;

	public float flingAcceleration;
	public float flingSpeed;
	public Vector2 flingDirection;

	// Use this for initialization
	void Awake () {
		_rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (_rb.velocity.magnitude >= flingSpeed) {
			_rb.velocity = Vector2.ClampMagnitude(_rb.velocity, flingSpeed);
			EndForce();
		}
		else {
			_rb.AddForce(flingDirection * flingAcceleration * _rb.mass);
		}
	}

	public void SetFlingProperties(Vector2 dir, float speed, float acceleration) {
		_originalGravityScale = _rb.gravityScale;
		_rb.gravityScale = 0;
		_rb.velocity = Vector2.zero;

		flingDirection = dir;
		flingSpeed = speed;
		flingAcceleration = acceleration;
	}

	void EndForce() {
		_rb.gravityScale = _originalGravityScale;
		Destroy(this);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		EndForce();	
	}
}
