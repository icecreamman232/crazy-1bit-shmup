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
    /// <summary>
    /// The number of monster in the list.
    /// </summary>
    public int numberMonsterList;
    public bool isWaveFinished;

    /// <summary>
    /// The number of monster finished their jobs, either died or finished their path.
    /// </summary>
    public int numberMonsterCompleteDuty;

    public int numMonsterRan;
    public float delayNextMonster;
    public float timer;
    public bool isRunning;
    public bool isSpawned;

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
        for (int i = 0; i < waveMonsterList.Count; i++)
        {
            waveMonsterList[i].monster.Setup();
        }

        isRunning = true;
        isSpawned = false;

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

    private void Update()
    {
        while(isRunning)
        {
            if (!isSpawned)
            {
                waveMonsterList[numMonsterRan].monster.Spawn();

                isSpawned = true;
            }
            timer += Time.deltaTime;
            if (timer >= waveMonsterList[numMonsterRan].delayTime)
            {
                timer = 0;
                numMonsterRan++;
                isSpawned = false;
                if (numMonsterRan >= numberMonsterList)
                {
                    numMonsterRan = 0;
                    isRunning = false;
                }
            }
        }
    }

    #region Editor
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Draw wireframe of screen ratio for better wave designing

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
