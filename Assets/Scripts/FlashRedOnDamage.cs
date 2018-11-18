using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class FlashRedOnDamage : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	private Coroutine curCoroutine;
	public float animationTime;

	private Color _startColor;
	private bool _isFlashing = false;

	// Use this for initialization
	void Start () {
		GetComponent<Health>().OnDamaged.AddListener(FlashRed);
		_startColor = spriteRenderer.color;

	}
	
	void FlashRed() {
		if (_isFlashing && curCoroutine != null) {
			StopCoroutine(curCoroutine);
		}

		curCoroutine = StartCoroutine(FlashColor(Color.red, animationTime));
	}

	IEnumerator FlashColor(Color col, float animTime) {
		_isFlashing = true;
		spriteRenderer.color = col;
		float time = 0;
		while(time < animTime) {
			time += Time.deltaTime;
			spriteRenderer.color = Color.Lerp(col, _startColor, time / animTime);
			yield return null;
		}

		spriteRenderer.color = _startColor;
		_isFlashing = false;
	}
}
