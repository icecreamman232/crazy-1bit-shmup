using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CollectorRankDO",menuName ="Data Objects/Collector Rank",order =1)]
public class CollectorRankDO : ScriptableObject
{
    public List<CollectorRank> list_collector_rank;
}

[Serializable]
public class CollectorRank
{
    public int rank_id;
    public int exp_needed;
    public Sprite rank_sprite;
}

