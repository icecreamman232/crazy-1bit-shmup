using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveMonsterHolder
{
    public MonsterWithCustomPath monster;
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

    public int numberMonsterCompleteDuty;

    private void OnEnable()
    {
        for (int i = 0; i < waveMonsterList.Count; i++)
        {
            //Add callback for getting detroyed event
            waveMonsterList[i].monster.OnDie += OnCompleteCycle;
            //Add callback for finishing path event
            waveMonsterList[i].monster.OnFinishRun += OnCompleteCycle;
        }
    }
    public void Run()
    {
        numberMonsterCompleteDuty = 0;
        numberMonsterList = waveMonsterList.Count;
        isWaveFinished = false;


        StartCoroutine(OnSpawningMonster());
    }
    public void Reset()
    {
        for (int i = 0; i < waveMonsterList.Count; i++)
        {
            //Unsubscribe events to prevent memory leak
            waveMonsterList[i].monster.OnDie -= OnCompleteCycle;
            waveMonsterList[i].monster.OnFinishRun -= OnCompleteCycle;

            waveMonsterList[i].monster.bezierMoveController.Stop();
        }
        Lean.Pool.LeanPool.Despawn(this.gameObject);
    }
    private void OnCompleteCycle()
    {
        numberMonsterCompleteDuty++;
        if(numberMonsterCompleteDuty >= waveMonsterList.Count)
        {
            isWaveFinished = true;
            Reset();
        }
    }

    private IEnumerator OnSpawningMonster()
    {
        for (int i = 0; i < waveMonsterList.Count; i++)
        {
            waveMonsterList[i].monster.Setup();
            waveMonsterList[i].monster.Spawn();
            yield return new WaitForSeconds(waveMonsterList[i].delayTime);
        }
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
