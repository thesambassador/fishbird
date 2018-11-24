using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public Vector2 direction;
	public float lifespan;

	public float speed;
	public AnimationCurve velocityCurve;

	public Rigidbody2D rb;

	public bool destroyOnCollision;

	// Use this for initialization
	void OnEnable () {
		StartCoroutine(ProjectileCoroutine());
	}

	void OnDisable() {
		StopAllCoroutines();
	}

	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator ProjectileCoroutine()
	{
		float time = 0;
		while(time < lifespan) {
			float velocity = velocityCurve.Evaluate(time / lifespan) * speed;

			rb.velocity = velocity * direction;

			time += Time.deltaTime;
			yield return null;
		}
		ObjectPoolManager.ReturnObject(this.gameObject);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (destroyOnCollision) {
			ObjectPoolManager.ReturnObject(this.gameObject);
		}
	}
}
