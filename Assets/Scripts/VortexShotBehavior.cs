using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexShotBehavior : MonoBehaviour {
	public Rigidbody2D rb;

	public Vector2 direction;
	public float projectileSpeed = 10;
	public float postReleaseLifespan = 3;
	public float minDistBeforeRelease;
	public float maxDist = 5;

	public bool shouldRelease = false;
	public bool released = false;

	public AnimationCurve releasedVelocityCurve;

	private float _fullSpeed;
	private float _curSpeed;
	private float _curLifespan;
	private Vector2 _startPos;

	// Update is called once per frame
	void Update () {
		float dist = Vector2.Distance(transform.position, _startPos);

		if(dist > maxDist) {
			released = true;
		}

		if (shouldRelease) {
			if(dist > minDistBeforeRelease) {
				released = true;
			}
		}

		if (released) {
			_curLifespan -= Time.deltaTime;
			_curSpeed = _fullSpeed * releasedVelocityCurve.Evaluate(_curLifespan / postReleaseLifespan);
			//print(_curSpeed + ", " + _curLifespan);
			if(_curLifespan < 0) {
				ObjectPoolManager.ReturnObject(this.gameObject);
			}

		}

		rb.velocity = _curSpeed * direction;
	}

	public void Shoot(Vector2 dir, float sourceSpeed, float releaseDist) {
		shouldRelease = false;
		released = false;
		_curLifespan = postReleaseLifespan;
		_startPos = transform.position;

		_fullSpeed = sourceSpeed + projectileSpeed;
		_curSpeed = _fullSpeed;
		direction = dir;
		minDistBeforeRelease = releaseDist;
	}


	public void ReleaseVortexShot() {
		shouldRelease = true;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		print(collision.gameObject.name);
		if (collision.tag == "Terrain" || collision.tag == "Obstacle") {
			ReleaseVortexShot();
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if(collision.tag == "Water") {
			ReleaseVortexShot();
			_curLifespan = postReleaseLifespan * .5f;
		}
	}
}
