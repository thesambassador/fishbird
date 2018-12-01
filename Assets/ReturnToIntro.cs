using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToIntro : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(ReturnCoroutine());
	}

	IEnumerator ReturnCoroutine() {
		yield return new WaitForSeconds(3); 
		SceneManager.LoadScene(0);
	}
	
}
