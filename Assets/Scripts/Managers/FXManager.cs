using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ID 1 = Generic Die Expliosion
 * ID 2 = Explosion using for transform meteor monster
 */

[System.Serializable]
public struct FXItem
{
    public int id;
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
        if(itemArr.Length > 0)
        {
            for (int i = 0; i < itemArr.Length; i++)
            {
                fxDictionary.Add(itemArr[i].id, itemArr[i].fxItem);
            }
        }
    }

    public void CreateFX(int id,Vector3 pos)
    {
        if(fxDictionary.ContainsKey(id))
        {
            GameObject fxItem = fxDictionary[id];

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
