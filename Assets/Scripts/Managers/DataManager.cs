using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        LoadDataFromSave();
    }
    /// <summary>
    /// Tổng tiền hiện có của người chơi
    /// </summary>
    public int gold_total_value;

    public void UpdateGold(int new_gold_amount)
    {
        gold_total_value += new_gold_amount;
    }
    public void LoadDataFromSave()
    {
        //Về sau thì load lên từ save, hiện tại thì reset về 0
        gold_total_value = 0;
    }
}
