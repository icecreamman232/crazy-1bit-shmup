using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PoolType
{
    public string tag;
    public int capacity;
    public GameObject prefab;
}


public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public List<PoolType> poolList;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        for(int i = 0; i < poolList.Count; i++)
        {
            Queue<GameObject> objQueue = new Queue<GameObject>();
            for(int j = 0; j < poolList[i].capacity; j++)
            {
                GameObject newObj = Instantiate(poolList[i].prefab);
                newObj.name = poolList[i].tag + "_" + j.ToString();
                newObj.SetActive(true);
                objQueue.Enqueue(newObj);
            }
            poolDictionary.Add(poolList[i].tag, objQueue);
        }
    }

    public GameObject Spawn(string tag, Vector3 position,Quaternion rotation, Transform parent = null)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            return null;
        }
        GameObject obj = poolDictionary[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.parent = parent;
        poolDictionary[tag].Enqueue(obj);
        return obj;
    }
    public void Despawn(string tag,GameObject obj)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            return;
        }
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(false);
    }
}

