using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMonsterController : MonoBehaviour
{
    public List<MonsterWithSplineMove> monsterList;

    public int numberMonsterList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Run()
    {
        numberMonsterList = monsterList.Count;
        for (int i = 0; i < monsterList.Count; i++)
        {
            monsterList[i].OnDie.AddListener(ThereisMonsterDied);
        }
        StartCoroutine(OnMonsterRunning());
    }
    IEnumerator OnMonsterRunning()
    {
        for(int i  = 0; i < monsterList.Count; i++)
        {
            monsterList[i].Run(); 
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void ThereisMonsterDied()
    {
        numberMonsterList--;
        if(numberMonsterList <= 0)
        {
            Lean.Pool.LeanPool.Despawn(this.gameObject);
        }
    }
}
