using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualParent : MonoBehaviour {

	public Transform target;

	Vector2 offset;

	// Use this for initialization
	void Start () {
		offset = target.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = (Vector2)target.position - offset;
	}
}
