using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ID 1 = Generic Die Expliosion
 * ID 2 = Explosion using for transform meteor monster
 */

[System.Serializable]
public enum fxID
{
    DIE_MONSTER_EXPLOSION = 1,
    EXPLOSION_FOR_TRANSFORM = 2,
    DIE_SHIP_EXPLOSION = 3,
}

[System.Serializable]
public struct FXItem
{
    public fxID fxID;
    public GameObject fxItem;
}

public class FXManager : MonoBehaviour
{
    public FXItem[] itemArr;
    public Dictionary<int, GameObject> fxDictionary;
    public static FXManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        fxDictionary = new Dictionary<int, GameObject>();
        fxDictionary.Clear();
        if (itemArr.Length > 0)
        {
            for (int i = 0; i < itemArr.Length; i++)
            {
                fxDictionary.Add((int)itemArr[i].fxID, itemArr[i].fxItem);
            }
        }
    }

    public void CreateFX(fxID id, Vector3 pos)
    {
        int integerID = (int)id;
        if (fxDictionary.ContainsKey(integerID))
        {
            GameObject fxItem = fxDictionary[integerID];

            var obj = Lean.Pool.LeanPool.Spawn(fxItem, pos, Quaternion.identity);
            obj.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DespawnFX(obj));
        }
    }

    private IEnumerator DespawnFX(GameObject fxItem)
    {
        yield return new WaitUntil(() => fxItem.GetComponent<ParticleSystem>().isEmitting == false);
        Lean.Pool.LeanPool.Despawn(fxItem);
    }
}