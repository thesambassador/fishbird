using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class Health : MonoBehaviour {
	public float hp;
	public float invulnTimeAfterHit = 0;
	public GameObject killPrefab;

	private float _invulnTimer;

	public UnityEvent OnDamaged;
	public UnityEvent OnKilled;

	public bool Invulnerable
	{
		get
		{
			return _invulnTimer > 0;
		}
	}

	void Awake() {
		if(OnDamaged == null) {
			OnDamaged = new UnityEvent();
		}
		if(OnKilled == null) {
			OnKilled = new UnityEvent();
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(_invulnTimer > 0) {
			_invulnTimer -= Time.deltaTime;
		}
	}

	public void Damage(float damageAmount) {
		if (!Invulnerable) {
			hp -= damageAmount;
			if(hp <= 0) {
				Kill();
			}
			_invulnTimer = invulnTimeAfterHit;

			if(OnDamaged != null) {
				OnDamaged.Invoke();
			}
		}
	}

	public void Kill() {
		if (OnKilled != null) {
			OnKilled.Invoke();
		}

		if(killPrefab != null) {
			Instantiate(killPrefab, transform.position, transform.rotation);
		}
		Destroy(this.gameObject);
	}

}
