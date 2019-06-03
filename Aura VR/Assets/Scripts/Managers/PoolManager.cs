using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region Singleton
    private static PoolManager _instance;

    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new PoolManager();

            return _instance;
        }
    }
    #endregion

    Dictionary<string, Pool> _poolDictionary;

    private PoolManager()
    {
        _poolDictionary = new Dictionary<string, Pool>();
    }

    public void CreatePool(string poolID, GameObject prefab, int poolSize)
    {
        if (!_poolDictionary.ContainsKey(poolID))
        {
            Pool pool = new Pool(poolID, prefab, poolSize);
            _poolDictionary.Add(poolID, pool);
        }
    }

    public void RemovePool(string poolID)
    {
        if (_poolDictionary.ContainsKey(poolID))
        {
            // Empty pool
            Pool pool = _poolDictionary[poolID];
            pool.EmptyPool();

            _poolDictionary.Remove(poolID);
        }
    }

    public GameObject SpawnFromPool(string poolID, Vector3 position, Quaternion rotation)
    {
        if (!_poolDictionary.ContainsKey(poolID))
        {
            Debug.LogWarning("Pool with the ID " + poolID + " doesn't exist");
            return null;
        }

        GameObject objectToSpawn = _poolDictionary[poolID].SpawnFromPool();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }
}

class Pool
{
    private string _poolID;
    private Queue<GameObject> _objectPool;
    private int _poolSize;

    public Pool(string poolID, GameObject prefab, int poolSize)
    {
        _poolID = poolID;
        _objectPool = new Queue<GameObject>();
        _poolSize = poolSize;

        for (int i = 0; i < poolSize; i += 1)
        {
            GameObject newObject = Photon.Pun.PhotonNetwork.Instantiate(prefab.name, Vector3.zero, Quaternion.identity);
            newObject.SetActive(false);

            _objectPool.Enqueue(newObject);
        }
    }

    public GameObject SpawnFromPool()
    {
        GameObject objectToSpawn = _objectPool.Dequeue();
        objectToSpawn.SetActive(true);
        _objectPool.Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void EmptyPool()
    {
        // Ensure all objects within are deleted
        foreach (GameObject gameObject in _objectPool)
        {
            Object.Destroy(gameObject);
        }
        _objectPool.Clear();
    }
}
