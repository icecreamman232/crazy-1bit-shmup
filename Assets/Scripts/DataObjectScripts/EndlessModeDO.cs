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

    /// <summary>
    /// Sau wave milestone thì sẽ có biến đổi về tần suất spawn quái
    /// </summary>
    [Header("Monster Spawn Rate")]
    public float min_delay_limit;
    public float max_delay_limit;

    public float min_decrease_rate;
    public float max_decrease_rate;

    public List<int> wave_milestone;

    

    [Header("Coin Design")]
    public float coin_increase_per_wave;

    [Header("HP Design")]
    public float hp_increase_per_wave;

}
