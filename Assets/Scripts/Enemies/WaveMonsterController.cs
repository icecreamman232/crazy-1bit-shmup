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
    public int currentIndexMonster;


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
        currentIndexMonster = 0;
        isWaveFinished = false;
        for (int i = 0; i < waveMonsterList.Count; i++)
        {
            //Add event if monster died
            waveMonsterList[i].monster.OnDie += ThereisMonsterDied;

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
    public void ThereisMonsterDied()
    {
        currentIndexMonster++;
        numberMonsterList--;
        Debug.Log("Dead State, Monster Left=" + numberMonsterList);
        if(numberMonsterList <= 0)
        {
            isWaveFinished = true;
            for (int i = 0; i < waveMonsterList.Count; i++)
            {
               
                waveMonsterList[i].monster.splineMove.Stop();
            }
            Lean.Pool.LeanPool.Despawn(this.gameObject);
        }
    }
    public void FinishedRun()
    {
        currentIndexMonster++;
        numberMonsterList--;
        Debug.Log("Finished Run, Monster Left=" + numberMonsterList);
        if (numberMonsterList <=0)
        {
            isWaveFinished = true;
            Lean.Pool.LeanPool.Despawn(this.gameObject);
        }
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
