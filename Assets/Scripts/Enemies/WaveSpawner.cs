using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class WaveSpawner : MonoBehaviour
{
    public LevelDesignDO levelDesignDO;
    public WaveMonsterController currentWaveMonster;

    public int waveIndex;
    public int maxWave;

    private bool isSpawning;
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
    private void Update()
    {
        
    }
    private IEnumerator OnWaveRunning()
    {
        while(true)
        {
            if(!isSpawning)
            {
                yield break;
            }
            currentWaveMonster = LeanPool.Spawn(levelDesignDO.monsterList[waveIndex].waveMonster, this.transform);
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
