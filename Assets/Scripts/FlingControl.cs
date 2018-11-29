using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;

public class FlingControl : MonoBehaviour {

	List<Flingable> flingCandidates;
	Flingable[] allFlingables;

	public int playerId;

	Rigidbody2D rb;
	Player player;
	PlayerMovement pm;

	Flingable target;

	public float flingSpeed;
	public float flingAcceleration;
	public float flingableDetectionRadius;

	public float timeSlow = .25f;

	bool determiningDirection = false;

	// Use this for initialization
	void Start () {
		allFlingables = FindObjectsOfType<Flingable>();
		flingCandidates = new List<Flingable>();
		player = ReInput.players.GetPlayer(playerId);
		pm = GetComponent<PlayerMovement>();
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (pm.state == PlayerMovementState.FLYING) {
			GetCandidateFlingables();
			if (flingCandidates.Count > 0) {
				target = GetBestFlingable();
				HighlightTarget(target);

				if (!determiningDirection && player.GetButtonDown("Move Ability")) {
					determiningDirection = true;
					StartCoroutine(SlowTime(timeSlow));
				}

				if (determiningDirection) {
					Vector2 aimVector = player.GetAxis2D("Horizontal", "Vertical");
					pm.aimDirection = aimVector.normalized;

					if (player.GetButtonUp("Move Ability")) {
						target.Fling(rb, -aimVector.normalized, flingSpeed, flingAcceleration);
						target = null;
						determiningDirection = false;
					}
				}
			}
			else {
				determiningDirection = false;
				target = null;
			}
		}

		
	}

	void GetCandidateFlingables() {
		flingCandidates = new List<Flingable>();
		foreach(Flingable flingable in allFlingables) {
			if(Vector2.Distance(transform.position, flingable.transform.position) < flingableDetectionRadius)
			flingCandidates.Add(flingable);
		}
	}

	Flingable GetBestFlingable() {
		Flingable result = null;
		float smallestDist = 10000;

		foreach(Flingable flingable in flingCandidates) {
			float dist = Vector2.Distance(transform.position, flingable.transform.position);
			if(dist < smallestDist) {
				smallestDist = dist;
				result = flingable;
			}
		}

		return result;
	}
	
	void HighlightTarget(Flingable target) {

	}

	IEnumerator SlowTime(float newTimescale) {
		Time.timeScale = newTimescale;

		float originalFixed = Time.fixedDeltaTime;
		Time.fixedDeltaTime = Time.fixedDeltaTime * newTimescale;

		while (determiningDirection) {
			yield return null;
		}

		Time.timeScale = 1;
		Time.fixedDeltaTime = originalFixed;
	}

	//private void OnTriggerEnter2D(Collider2D collision) {
	//	Flingable flingable = collision.GetComponent<Flingable>();
	//	if (flingable != null) {
	//		flingCandidates.Add(flingable);
	//	}
	//}

	//private void OnTriggerExit2D(Collider2D collision) {
	//	Flingable flingable = collision.GetComponent<Flingable>();
	//	if(flingable != null) {
	//		flingCandidates.Remove(flingable);
	//	}
	//}


}
