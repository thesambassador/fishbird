using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableCollectible : MonoBehaviour {
	public int score;

	public void Break() {
		GameManager.instance.AddScore(score);
	}
}
