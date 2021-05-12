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
            //Add event if monster died
            monsterList[i].OnDie.AddListener(ThereisMonsterDied);

            //Add event if monster go to end point on the path
            monsterList[i].splineMove.movementEndEvent += ThereisMonsterDied;
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
            OnDead();
        }
    }
    public void OnDead()
    {
        Lean.Pool.LeanPool.Despawn(this.gameObject);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //9:16
        Gizmos.DrawWireCube(transform.position, new Vector3(2.9f*2, 5.1f*2));
        Gizmos.color = Color.blue;
        //9:18
        Gizmos.DrawWireCube(transform.position, new Vector3(2.9f * 2, 5.8f*2));
    }
#endif
}
