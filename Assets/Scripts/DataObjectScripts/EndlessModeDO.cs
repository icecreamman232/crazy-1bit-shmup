using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EndlessModeDataObject", menuName = "Data Objects/Endless Mode Data", order = 0)]
public class EndlessModeDO : ScriptableObject
{
    [Header("Speed Design")]
    public float speedIncreasePerWave;
    public float minSpeed;
    public float maxSpeed;
    public float speedLimit;


    [Header("Coin Design")]
    public float coinIncreasePerWave;

    [Header("HP Design")]
    public float HPIncreasePerWave;

}
