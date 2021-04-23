using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EndlessModeDataObject", menuName = "Game Mode DO/Endless Mode", order = 0)]
public class EndlessModeDO : ScriptableObject
{
    public float speed_increase_per_wave;

    public float min_speed;
    public float max_speed;
}
