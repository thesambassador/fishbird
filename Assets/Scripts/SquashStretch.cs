using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashStretch : MonoBehaviour {
	private Vector2 lastPos;
	private Vector2 lastVel;

	public float XYScaleRatio = 1;
	public AnimationCurve AccelerationScaleCurve;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 vel = (Vector2)transform.position - lastPos;
		Vector2 acc = vel - lastVel;

		if (vel.magnitude > 0) {
			transform.right = vel.normalized;
		}

		float accelerationSignedMagnitude = Mathf.Sign(Vector2.Dot(vel, acc)) * acc.magnitude;

		XYScaleRatio = AccelerationScaleCurve.Evaluate(accelerationSignedMagnitude);

		Vector2 newScale = new Vector2(XYScaleRatio, 1 / XYScaleRatio);
		//transform.localScale = newScale;

		lastPos = (Vector2)transform.position;
		lastVel = vel;
	}
}
