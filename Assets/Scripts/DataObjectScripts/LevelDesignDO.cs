using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="LevelDesign",menuName ="Data Objects/Level",order =2)]
public class LevelDesignDO : ScriptableObject
{
    public List<WaveMonsterCointainer> monsterList;
}


[System.Serializable]
public class WaveMonsterCointainer
{
    public WaveMonsterController waveMonster;
    public float delayNextWave;
}
