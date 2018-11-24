using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabNumPairing
{
    public GameObject Prefab;
    public int InitPoolSize;
}

public class ObjectPoolInitializer : MonoBehaviour {
    public bool CreateParentTransforms = true;
    public PrefabNumPairing[] InitialPrefabs;
    

	// Use this for initialization
	void Start () {
        for (int i = 0; i < InitialPrefabs.Length; i++)
        {
            Transform parentTransform = null;
            if (CreateParentTransforms)
            {
                GameObject go = new GameObject();
                go.name = InitialPrefabs[i].Prefab.name;
                go.transform.parent = ObjectPoolManager.Instance.transform;
                parentTransform = go.transform;
            }

            ObjectPoolManager.CreatePool(InitialPrefabs[i].Prefab, InitialPrefabs[i].InitPoolSize, parentTransform);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
