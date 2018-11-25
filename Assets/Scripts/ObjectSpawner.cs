using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//simple script to spawn objects in, then respawn them if necessary
//basically, if the checkpointLocation is equal to or above the current checkpoint
//and we "reset" the level, it resets the objects

public class ObjectSpawner : MonoBehaviour {

	public GameObject prefabToSpawn;
	public int checkpointLocation;
	public bool despawnIfPassedCheckpoint = false;

	private GameObject _spawnedObject;

	// Use this for initialization
	void Start () {
		GameManager.instance.OnLevelReset.AddListener(OnLevelReset);
		GameManager.instance.OnCheckpointEntered.AddListener(OnCheckpointReached);

		SpawnObject();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//only spawn if the spawned object is null or not active in the hierarchy
	public void SpawnObject() {
		if(_spawnedObject != null) {
			if (!_spawnedObject.activeInHierarchy) {
				_spawnedObject = ObjectPoolManager.GetObject(prefabToSpawn, transform.position, transform.rotation);
				_spawnedObject.transform.localScale = transform.localScale;
			}
		}
		else {
			_spawnedObject = ObjectPoolManager.GetObject(prefabToSpawn, transform.position, transform.rotation);
			_spawnedObject.transform.localScale = transform.localScale;
		}
	}

	public void ForceRespawn() {
		if(_spawnedObject != null && _spawnedObject.activeInHierarchy) {
			ObjectPoolManager.ReturnObject(_spawnedObject);
			_spawnedObject = null;
		}
		SpawnObject();
	}

	public void SetSpawnedObject(GameObject currentObject) {
		_spawnedObject = currentObject;
	}

	void OnLevelReset(int currentCheckpoint) {
		if(checkpointLocation >= currentCheckpoint) {
			ForceRespawn();
		}
	}

	void OnCheckpointReached(int checkpointNum) {
		if (despawnIfPassedCheckpoint) {
			if (checkpointNum > checkpointLocation) {
				if (_spawnedObject.activeInHierarchy) {
					ObjectPoolManager.ReturnObject(_spawnedObject);
					ObjectPoolManager.ReturnObject(this.gameObject);
				}
			}
		}
	}
}
