using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;

public class RespawnPlayerAfterDelay : MonoBehaviour {

	public float delay = 2;
	public bool requireButtonPress = false;

	private float _delayTimer = -1;
	private Player _player;

	private void Start() {
		_player = ReInput.players.GetPlayer(0);
	}

	void OnEnable () {
		_delayTimer = delay;
	}
	
	// Update is called once per frame
	void Update () {
		delay -= Time.deltaTime;
		if(delay < 0) {
			if (requireButtonPress) {
				if (_player.GetAnyButtonDown()) {
					GameManager.instance.ResetWorldToLastCheckpoint();
					ObjectPoolManager.ReturnObject(this.gameObject);
				}
			}
			else {
				GameManager.instance.ResetWorldToLastCheckpoint();
				ObjectPoolManager.ReturnObject(this.gameObject);
			}
		}
	}
}
