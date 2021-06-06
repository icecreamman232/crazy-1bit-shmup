using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This component can be used on any entity to excute dropping item behavior
 */


public class ItemSpawner : MonoBehaviour
{
    [Header("Configuration")]
    public List<ItemType> dropableItemList;
    /// <summary>
    /// Value từ 0 ->1000
    /// </summary>
    public int itemDropRate;
    private int maxItem;
    public GameObject itemPrefab;
    private void Start()
    {
        maxItem = dropableItemList.Count;
    }

    public void DropItem(Vector3 spawnPos)
    {
        Vector3 trajectory = Random.insideUnitCircle * 100.0f;
        var item = Lean.Pool.LeanPool.Spawn(itemPrefab, spawnPos, Quaternion.identity);
        var itemTypeIndex = Random.Range(0, maxItem);
        item.GetComponent<BaseItem>().SetupItem(dropableItemList[itemTypeIndex]);
        var force_vector = new Vector3(
            Random.Range(-100f, 100f) + trajectory.x,
            Random.Range(-100f, 300f) + trajectory.y,
            0f);
        item.GetComponent<Rigidbody2D>().AddForce(force_vector);
    }
}
