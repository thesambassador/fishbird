using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool {
    
    public GameObject Prefab;
    public Transform ParentWhileInactive;
    public bool CanGrow = true;
   
    private HashSet<GameObject> _pooledObjects;
    private HashSet<GameObject> _activeObjects;
    private int _poolSize;

    public int PoolSize{
        get{ return _poolSize;}
    }

    public ObjectPool(GameObject prefabToUse, int poolSize, Transform parentWhileInactive = null)
    {
        Prefab = prefabToUse;
        ParentWhileInactive = parentWhileInactive;

        _poolSize = poolSize;
        InitializePool();
    }

    public void SetPoolSize(int newSize)
    {
        if (newSize > _poolSize)
        {
            int newObjects = newSize - _poolSize;
            for (int i = 0; i < newObjects; i++)
            {
                InstantiatePoolObject();
            }
            _poolSize = newSize;
        }
        else
        {
            Debug.LogWarning("Object pool shrinking not implemented");
        }
    }

    public GameObject GetObject()
    {
        return GetObject(Vector3.zero, Quaternion.identity);
    }

    public GameObject GetObject(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (_pooledObjects.Count == 0)
        {
            if (CanGrow)
            {
                SetPoolSize(_poolSize + 1);
            }
            else
            {
                Debug.LogWarning("Pool for " + Prefab.name + " is empty and can't grow");
                return null;
            }
        }
        GameObject result = _pooledObjects.FirstOrDefault();

        result.SetActive(true);
        result.transform.position = position;
        result.transform.rotation = rotation;
        result.transform.parent = parent;

        _pooledObjects.Remove(result);
        _activeObjects.Add(result);

        return result;
    }

    public void ReturnObject(GameObject obj){
        obj.SetActive(false);
        if (_activeObjects.Contains(obj))
        {
            _activeObjects.Remove(obj);
            _pooledObjects.Add(obj);
            if (ParentWhileInactive != null)
                obj.transform.parent = ParentWhileInactive;
        }
        else
        {
            Debug.Log("Tried to return an object to pool " + Prefab.name + " that didn't start in that pool, or that object has already been returned");
        }
    }


    private void InitializePool()
    {
        _activeObjects = new HashSet<GameObject>();
        _pooledObjects = new HashSet<GameObject>();

        for (int i = 0; i < _poolSize; i++)
        {
            InstantiatePoolObject();
        }
    }

    private void InstantiatePoolObject()
    {
        GameObject newObject = GameObject.Instantiate(Prefab);
        if (ParentWhileInactive != null)
        {
            newObject.transform.parent = ParentWhileInactive;
        }
        newObject.SetActive(false);
        _pooledObjects.Add(newObject);
    }


}
