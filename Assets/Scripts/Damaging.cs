using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damaging : MonoBehaviour {

	public float damageAmount;
	[TagSelector]
	public string damageTag;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	private void OnCollisionEnter2D(Collision2D collision) {
		Health health = collision.gameObject.GetComponent<Health>();
		if (health != null) {
			if (damageTag != null && collision.gameObject.tag == damageTag) {
				health.Damage(damageAmount);
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision) {

	}
}
