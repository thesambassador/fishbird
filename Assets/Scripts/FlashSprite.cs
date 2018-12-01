using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashSprite : MonoBehaviour {
	public SpriteRenderer spriteRenderer;

	public Color flashColor;
	private Color _origColor;
	public float deflashSpeed;

	private float _flashLerp = 1;

	// Use this for initialization
	void Start () {
		_origColor = spriteRenderer.color;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F))
			Flash();
		if(_flashLerp < 1) {
			_flashLerp = Mathf.Min(1, _flashLerp + Time.deltaTime * deflashSpeed);
			spriteRenderer.color = Color.Lerp(flashColor, _origColor, _flashLerp);
		}
	}

	public void Flash() {
		_flashLerp = 0;
	}
}
