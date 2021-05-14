using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveMonster
{
    public MonsterWithSplineMove monster;
    public float delayToNextMonster;
}


public class WaveMonsterController : MonoBehaviour
{
    public List<WaveMonster> waveMonsterList;


    public int numberMonsterList;


    public bool isWaveFinished;

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
        numberMonsterList = waveMonsterList.Count;
        isWaveFinished = false;
        for (int i = 0; i < waveMonsterList.Count; i++)
        {
            //Add event if monster died
            waveMonsterList[i].monster.OnDie += FinishedRun;

            //Add event if monster go to end point on the path
            waveMonsterList[i].monster.splineMove.movementEndEvent += FinishedRun;
        }

        StartCoroutine(OnSpawningMonster());
    }
    IEnumerator OnSpawningMonster()
    {
        for (int i = 0; i < waveMonsterList.Count; i++)
        {
            waveMonsterList[i].monster.Run();
            yield return new WaitForSeconds(waveMonsterList[i].delayToNextMonster);
        }
    }
    public void FinishedRun()
    {
        if (isWaveFinished) return;
        if (numberMonsterList <= 0)
        {
            for (int i = 0; i < waveMonsterList.Count; i++)
            {
                //Unsubscribe events to prevent memory leak
                waveMonsterList[i].monster.OnDie -= FinishedRun;
                waveMonsterList[i].monster.splineMove.movementEndEvent -= FinishedRun;

                waveMonsterList[i].monster.splineMove.ResetToStart();
            }
            isWaveFinished = true;
            Lean.Pool.LeanPool.Despawn(this.gameObject);
        }
        numberMonsterList--;      
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

    public bool IsNullList()
    {
        for(int i = 0;i < waveMonsterList.Count; i++)
        {
            if(waveMonsterList[i].monster == null)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsThereUnusedPath()
    {
        for (int i = 0; i < waveMonsterList.Count; i++)
        {
            if (waveMonsterList[i].monster.splineMove.pathContainer==null)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsThereMonsterNotInTheList()
    {
        MonsterWithSplineMove[] listObjects = FindObjectsOfType<MonsterWithSplineMove>();
        if(listObjects.Length!= waveMonsterList.Count)
        {
            return true;
        }
        return false;
    }
#endif
}
