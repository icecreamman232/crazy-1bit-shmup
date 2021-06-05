﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public LevelDesignDO levelDesignDO;
    public WaveMonsterController currentWaveMonster;

    public int waveIndex;
    public int maxWave;

    private bool isSpawning;
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        
    }
    public void Setup()
    {
        waveIndex = 0;
        maxWave = levelDesignDO.monsterList.Count;
        currentWaveMonster = null;
        isSpawning = false;
    }
    public void Run()
    {
        isSpawning = true;
        StartCoroutine(OnWaveRunning());
    }

    public void Reset()
    {
        isSpawning = false;
        StopCoroutine(OnWaveRunning());
        currentWaveMonster.isWaveFinished = true;
        currentWaveMonster.Reset();
    }
    private IEnumerator OnWaveRunning()
    {
        while(true)
        {
            if(!isSpawning)
            {
                yield break;
            }
            currentWaveMonster = Lean.Pool.LeanPool.Spawn(levelDesignDO.monsterList[waveIndex].waveMonster, this.transform);
            currentWaveMonster.Run();         
            yield return new WaitUntil(()=> currentWaveMonster.isWaveFinished);
            yield return new WaitForSeconds(levelDesignDO.monsterList[waveIndex].delayNextWave);
            waveIndex++;
            //Safety looping iteration
            if (waveIndex >= maxWave)
            {
                waveIndex = 0;
            }
        }
    }
}
