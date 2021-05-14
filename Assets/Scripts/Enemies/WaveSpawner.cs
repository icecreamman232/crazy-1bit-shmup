using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public List<WaveMonsterController> waveMonsterControllers;
    public LevelDesignDO levelDesignDO;
    public WaveMonsterController currentWaveMonster;

    public int waveIndex;
    public int maxWave;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Setup()
    {
        waveIndex = 0;
        maxWave = levelDesignDO.monsterList.Count;
        currentWaveMonster = null;
    }
    public void Run()
    {
        StartCoroutine(OnWaveRunning());
    }

    public void Reset()
    {
        StopCoroutine(OnWaveRunning());
        currentWaveMonster.Reset();
    }
    IEnumerator OnWaveRunning()
    {
        while(true)
        {
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
