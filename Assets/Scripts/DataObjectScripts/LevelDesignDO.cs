using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="LevelDesign",menuName ="Data Objects/Level",order =2)]
public class LevelDesignDO : ScriptableObject
{
    public List<MonsterWaveDO> monsterList;
}

[System.Serializable]
public class MonsterWaveDO
{
    public WaveMonsterController waveMonster;
    public float delayTime;
}
