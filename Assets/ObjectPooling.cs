using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
}

public class ObjectPooling : MonoBehaviour
{
    #region Singleton
    public static ObjectPooling Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<Pool> pools;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        for(int poolIndex = 0; poolIndex < pools.Count; poolIndex++)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i = 0; i < pools[poolIndex].size; i++ )
            {
                GameObject obj = Instantiate(pools[poolIndex].prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pools[poolIndex].tag, objectPool);
        }
    }
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation,Transform parent = null)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
#if UNITY_EDITOR
            Debug.LogError("Pool Key doesnt exist");
#endif
            return null;
        }
        GameObject objToSpawn = poolDictionary[tag].Dequeue();

        objToSpawn.SetActive(true);
        if(parent !=null)
        {
            objToSpawn.transform.parent = parent;
        }
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objToSpawn);
        return objToSpawn;
    }
}
