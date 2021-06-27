using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName ="Monster Design",menuName ="Design/Monster Design",order = 0)]
public class MonsterDesignDO : ScriptableObject
{
    
}

[System.Serializable]
public class MonsterData
{
    public int baseHP;
}
