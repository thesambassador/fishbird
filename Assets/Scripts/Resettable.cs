using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//add to objects to create an object spawner for this object, to reset for a checkpoint
public class Resettable : MonoBehaviour {

	public int checkpointLocation;
	public GameObject prefab;

	// Use this for initialization
	void Start () {
		CreateObjectSpawner();
	}

	void CreateObjectSpawner() {
		GameObject spawnerObj = new GameObject();
		spawnerObj.transform.position = transform.position;
		spawnerObj.transform.rotation = transform.rotation;
		spawnerObj.transform.parent = transform.parent;
		spawnerObj.transform.localScale = transform.localScale;

		ObjectSpawner spawner = spawnerObj.AddComponent<ObjectSpawner>();
		spawner.SetSpawnedObject(this.gameObject);
		spawner.prefabToSpawn = prefab;
		spawner.checkpointLocation = checkpointLocation;
	}
	
}
