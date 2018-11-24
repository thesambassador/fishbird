using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
	public int checkpointNumber = 0;

	[TagSelector]
	public string tag;

	private Collider2D _collider;
	// Use this for initialization
	void Start () {
		_collider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.tag == tag) {
			_collider.enabled = false;
			GameManager.instance.CheckpointReached(this);
		}
	}
}
