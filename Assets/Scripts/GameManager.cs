using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using Com.LuisPedroFonseca.ProCamera2D;

public class IntEvent : UnityEvent<int> { }

public class GameManager : MonoBehaviour {
	public static GameManager instance;
	public GameObject playerPrefab;

	public ProCamera2D cameraController;

	public IntEvent OnCheckpointEntered;
	public IntEvent OnLevelReset;

	public int FishPlayerID = 0;
	public int BirdPlayerID = 0;

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
		if (instance == null) {
			DontDestroyOnLoad(this);
			instance = this;
		}
		else {
			Destroy(this.gameObject);
		}

		if (OnCheckpointEntered == null) {
			OnCheckpointEntered = new IntEvent();
		}
		if(OnLevelReset == null) {
			OnLevelReset = new IntEvent();
		}
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

		OnCheckpointEntered.Invoke(_lastCheckpointNumber);
	}

	public void AddScore(int scoreAmount) {
		_scoreSinceLastCheckpoint += scoreAmount;
	}

	public void RespawnPlayerAtLastCheckpoint() {
		Vector3 spawnPos = Vector3.zero;
		if (_lastCheckpoint != null) {
			spawnPos = _lastCheckpoint.transform.position;
		}
		
		PlayerMovement newPlayer = ObjectPoolManager.GetObject(playerPrefab).GetComponent<PlayerMovement>();
		newPlayer.transform.position = spawnPos;

	}

	public void ResetWorldToLastCheckpoint() {
		RespawnPlayerAtLastCheckpoint();

		_scoreSinceLastCheckpoint = 0;

		OnLevelReset.Invoke(_lastCheckpointNumber);
		//todo respawn stuff that needs to be reset?
	}

	public void StartGame() {
		SceneManager.LoadScene(1);
	}

}
