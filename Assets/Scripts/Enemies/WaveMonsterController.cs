using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveMonsterHolder
{
    public MonsterWithCustomPath/*MonsterWithSplineMove*/ monster;
    /// <summary>
    /// Delay Time to Next Monster
    /// </summary>
    [Tooltip("Delay time before spawnning next monster")]
    public float delayTime;
}


public class WaveMonsterController : MonoBehaviour
{
    public List<WaveMonsterHolder> waveMonsterList;
    public int numberMonsterList;
    public bool isWaveFinished;
    private void OnEnable()
    {
        for (int i = 0; i < waveMonsterList.Count; i++)
        {
            //Add event if monster died
            waveMonsterList[i].monster.OnDie += OnMonsterDied;          
        }
        //Add event if monster go to end point on the path
        waveMonsterList[waveMonsterList.Count - 1].monster.OnFinishRun += OnFinishRun;

    }
    private void OnDisable()
    {
 
    }
    public void Run()
    {
        numberMonsterList = waveMonsterList.Count;
        isWaveFinished = false;

        StartCoroutine(CheckDie());
        StartCoroutine(OnSpawningMonster());
    }
    public void Reset()
    {
        for (int i = 0; i < waveMonsterList.Count; i++)
        {
            //Unsubscribe events to prevent memory leak
            waveMonsterList[i].monster.OnDie -= OnMonsterDied;
            if (i == waveMonsterList.Count - 1)
            {
                waveMonsterList[i].monster.OnFinishRun -= OnFinishRun;
            }
            waveMonsterList[i].monster.moveController.ResetToStart();
        }
        Lean.Pool.LeanPool.Despawn(this.gameObject);
    }
    IEnumerator OnSpawningMonster()
    {
        for (int i = 0; i < waveMonsterList.Count; i++)
        {
            waveMonsterList[i].monster.Setup();
            yield return new WaitForSeconds(waveMonsterList[i].delayTime);
        }
    }

    public void OnFinishRun()
    {       
        isWaveFinished = true;
        for (int i = 0; i < waveMonsterList.Count; i++)
        {
            //Unsubscribe events to prevent memory leak
            waveMonsterList[i].monster.OnDie -= OnMonsterDied;
            if (i == waveMonsterList.Count - 1)
            {
                waveMonsterList[i].monster.OnFinishRun -= OnFinishRun;
            }
            waveMonsterList[i].monster.moveController.ResetToStart();
        }
        
        Lean.Pool.LeanPool.Despawn(this.gameObject);
    }

    public void OnMonsterDied()
    {   
        //Count remaining monster
        numberMonsterList--;
    }

    public IEnumerator CheckDie()
    {
        yield return new WaitUntil(() => numberMonsterList <= 0);
        isWaveFinished = true;
        Lean.Pool.LeanPool.Despawn(this.gameObject);
    }



    #region Editor
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
    #endregion
}
