using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Com.LuisPedroFonseca.ProCamera2D;

public class GameManager : MonoBehaviour {
	public static GameManager instance;
	public PlayerMovement playerPrefab;

	public ProCamera2D cameraController;

	public int score
	{
		get
		{
			return _scoreAtLastCheckpoint + _scoreSinceLastCheckpoint;
		}
	}

	private Checkpoint _lastCheckpoint;
	private int _scoreSinceLastCheckpoint = 0;
	private int _lastCheckpointNumber;
	private int _scoreAtLastCheckpoint = 0;

	private void Awake() {
		instance = this;
	}

	void Start() {

	}

	void Update() {

	}

	public void CheckpointReached(Checkpoint newCheckpoint) {
		_scoreAtLastCheckpoint = score;
		_scoreSinceLastCheckpoint = 0;
		_lastCheckpoint = newCheckpoint;
		_lastCheckpointNumber = newCheckpoint.checkpointNumber;
	}

	public void AddScore(int scoreAmount) {
		_scoreSinceLastCheckpoint += scoreAmount;
	}

	public void RespawnPlayerAtLastCheckpoint() {
		Vector3 spawnPos = _lastCheckpoint.transform.position;

		PlayerMovement newPlayer = Instantiate(playerPrefab);
		newPlayer.transform.position = spawnPos;

	}

	public void ResetWorldToLastCheckpoint() {
		RespawnPlayerAtLastCheckpoint();

		_scoreSinceLastCheckpoint = 0;

		//todo respawn stuff that needs to be reset?
	}

}
