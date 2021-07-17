using UnityEngine;

[CreateAssetMenu()]
public class MonsterDO : EntityDO
{
    [Tooltip("Base score get on kill")]
    public int baseScore;

    public ItemDropDO itemData;
}
