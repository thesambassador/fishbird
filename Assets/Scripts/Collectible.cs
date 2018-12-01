using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

	public int points;

	public GameObject collectPointsEffect;

	[TagSelector]
	public string tagCheck;

	void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == tagCheck) {
			GameManager.instance.AddScore(points);

			if(collectPointsEffect != null) {
				GameObject effect = ObjectPoolManager.GetObject(collectPointsEffect, transform.position, transform.rotation);
				ObjectPoolManager.ReturnObject(effect, 1);
			}

			ObjectPoolManager.ReturnObject(this.gameObject);
		}
	}

}
