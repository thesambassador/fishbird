using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flingable : MonoBehaviour {

	private Rigidbody2D _rb;

	// Use this for initialization
	void Start () {
		_rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Fling(Rigidbody2D other, Vector2 direction, float flingSpeed, float flingAcceleration) {
		FlingForceComponent ffcThis = gameObject.AddComponent<FlingForceComponent>();
		ffcThis.SetFlingProperties(direction, flingSpeed, flingAcceleration);

		FlingForceComponent ffcOther = other.gameObject.AddComponent<FlingForceComponent>();
		ffcOther.SetFlingProperties(-direction, flingSpeed, flingAcceleration);
	}

}
