using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlingableSlot : MonoBehaviour {

	public Rigidbody2D rb;

	public Flingable slottedFlingable;

	public float cooldownAfterFling;

	public bool onCooldown = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void SlotFlingable(Flingable newFlingable) {
		newFlingable.flingSlot = this;
		slottedFlingable = newFlingable;
		slottedFlingable.springJoint.connectedBody = rb;
		slottedFlingable.springJoint.connectedAnchor = Vector2.zero;
		//slottedFlingable.springJoint.breakForce = Mathf.Infinity;
		slottedFlingable.springJoint.distance = 0.005f;
		slottedFlingable.springJoint.enabled = true;
		//StartCoroutine(SetBreakableForce());
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if(slottedFlingable == null && !onCooldown) {
			Flingable flingableCheck = collision.GetComponent<Flingable>();
			if (flingableCheck != null && flingableCheck.slottable) {
				SlotFlingable(flingableCheck);
			}
		}
	}

	public void UnslotFlingable() {
		slottedFlingable = null;
		StartCoroutine(CooldownSlot(cooldownAfterFling));
	}

	IEnumerator CooldownSlot(float time) {
		onCooldown = true;
		yield return new WaitForSeconds(time);
		onCooldown = false;
	}
}
