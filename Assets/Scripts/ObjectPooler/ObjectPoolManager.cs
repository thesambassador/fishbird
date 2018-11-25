using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour {

    private static ObjectPoolManager _instance;

    //Here the key is the prefab, not a specific instance.  
    private Dictionary<GameObject, ObjectPool> _pools;

    //This is for returning objects, so we can make sure we are returning objects to the correct pool
    //Note that I could have just used the name of the prefab as the key to avoid needing this, but then:
    //I'd have to rename objects when warming pools, you could never rename an object from a pool, and
    //in theory the string hashing function is slightly slower than just using an int (which is what Gameobject does)
    private Dictionary<GameObject, ObjectPool> _activeObjectLookup;

    public static ObjectPoolManager Instance
    {
        get{
            return _instance;
        }
    }


    void Awake()
    {
        //Right now, I've decided to not have the pooler persist across scenes, so if we load a new scene,
        //We'll make sure that we destroy the old pooler and overwrite it with the new one
        if (_instance != null)
        {
            if(_instance.isActiveAndEnabled)
                Destroy(_instance.gameObject);
        }
        _instance = this;
        Initialize();
    }

    void Initialize()
    {
        _pools = new Dictionary<GameObject, ObjectPool>();
        _activeObjectLookup = new Dictionary<GameObject, ObjectPool>();
    }

    public static void CreatePool(GameObject prefab, int poolSize, Transform parentWhileInactive = null)
    {
        _instance._pools[prefab] = new ObjectPool(prefab, poolSize, parentWhileInactive);
    }

    public static GameObject GetObject(GameObject prefab)
    {
        return GetObject(prefab, Vector3.zero, Quaternion.identity);
    }

    public static GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (_instance._pools.ContainsKey(prefab))
        {
            ObjectPool pool = _instance._pools[prefab];
            GameObject result = pool.GetObject(position, rotation, parent);

            if (result != null)
            {
                _instance._activeObjectLookup.Add(result, pool);
            }

            return result;
        }
        else
        {
            Debug.LogWarning("Object " + prefab.name + " does not have a pool, instantiating instead");
            return GameObject.Instantiate(prefab, position, rotation, parent);
        }
    }

    public static void ReturnObject(GameObject activeObject)
    {
        if (_instance._activeObjectLookup.ContainsKey(activeObject))
        {
            _instance._activeObjectLookup[activeObject].ReturnObject(activeObject);
            _instance._activeObjectLookup.Remove(activeObject);
        }
        else
        {
            //Debug.LogWarning("Object " + activeObject.name + " has no pool to return to, destroying instead");
            Destroy(activeObject.gameObject);
        }
    }


}
