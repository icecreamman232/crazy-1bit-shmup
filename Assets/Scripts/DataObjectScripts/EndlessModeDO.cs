using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EndlessModeDataObject", menuName = "Game Mode DO/Endless Mode", order = 0)]
public class EndlessModeDO : ScriptableObject
{
    [Header("Speed Design")]
    public float speed_increase_per_wave;
    public float min_speed;
    public float max_speed;
    public float speed_limit;
    [Header("Coin Design")]
    public float coin_increase_per_wave;

}
