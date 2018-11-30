using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;

public class FlingControl : MonoBehaviour {
	private static List<Flingable> _allFlingables;
	public static List<Flingable> AllFlingables
	{
		get
		{
			if(_allFlingables == null) {
				_allFlingables = new List<Flingable>();
			}
			return _allFlingables;
		}
	}


	private List<Flingable> flingCandidates;
	Rigidbody2D rb;
	PlayerMovement playerMovement;
	Collider2D collider2d;

	Flingable target;

	public float flingSpeed;
	public float flingAcceleration;
	public float flingableDetectionRadius;

	public float timeSlow = .25f;

	bool determiningDirection = false;

	Flingable currentHighlight;

	// Use this for initialization
	void Start () {
		flingCandidates = new List<Flingable>();
		playerMovement = GetComponent<PlayerMovement>();
		rb = GetComponent<Rigidbody2D>();
		collider2d = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (playerMovement.state == PlayerMovementState.FLYING) {
			GetCandidateFlingables();
			if (flingCandidates.Count > 0) {
				target = GetBestFlingable();
				HighlightTarget(target);

				if (!determiningDirection && playerMovement.fishPlayer.GetButtonDown("SecondaryAbility")) {
					determiningDirection = true;
					target.focused = true;
					StartCoroutine(SlowTime(timeSlow));
					StartCoroutine(TempDisableCollision(target));
				}

				if (determiningDirection) {
					Vector2 aimVector = playerMovement.fishPlayer.GetAxis2D("Horizontal", "Vertical");
					if (aimVector.magnitude == 0) {
						aimVector = playerMovement.aimDirection;
					}

					playerMovement.aimDirection = aimVector.normalized;

					transform.position = Vector2.Lerp(transform.position, target.transform.position, .1f);

					if (playerMovement.fishPlayer.GetButtonUp("SecondaryAbility")) {
						target.Fling(rb, -aimVector.normalized, flingSpeed, flingAcceleration);
						determiningDirection = false;
						HighlightTarget(null);
					}
				}
			}
			else {
				determiningDirection = false;
				target = null;
				HighlightTarget(null);
			}
		}
		else {
			HighlightTarget(null);
			determiningDirection = false;
			target = null;
		}

		
	}

	IEnumerator TempDisableCollision(Flingable flingable) {
		Physics2D.IgnoreCollision(flingable.collider, collider2d, true);
		print("disabled collision between " + this.name + " and " + flingable.name);
		while (!flingable.CanFling || flingable.focused) {
			yield return null;
		}
		print("re-enabled collision between " + this.name + " and " + flingable.name);
		Physics2D.IgnoreCollision(flingable.collider, collider2d, false);
	}

	void GetCandidateFlingables() {
		flingCandidates = new List<Flingable>();
		foreach(Flingable flingable in AllFlingables) {
			if(flingable.CanFling && Vector2.Distance(transform.position, flingable.transform.position) < flingableDetectionRadius) {
				flingCandidates.Add(flingable);
			}
			
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
	
	void HighlightTarget(Flingable newHighlight) {
		if(currentHighlight != null) {
			currentHighlight.Highlight(false);
		}
		currentHighlight = newHighlight;

		if (newHighlight != null) {
			newHighlight.Highlight(true);
		}
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
