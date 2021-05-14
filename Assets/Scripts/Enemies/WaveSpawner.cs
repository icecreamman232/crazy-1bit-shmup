using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public List<WaveMonsterController> waveMonsterControllers;
    public LevelDesignDO levelDesignDO;
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
    }
    public void Run()
    {
        StartCoroutine(OnWaveRunning());
    }
    IEnumerator OnWaveRunning()
    {
        while(true)
        {
            var wave = Lean.Pool.LeanPool.Spawn(levelDesignDO.monsterList[waveIndex].waveMonster, this.transform);
            wave.Run();         
            yield return new WaitUntil(()=> wave.isWaveFinished);
            yield return new WaitForSeconds(levelDesignDO.monsterList[waveIndex].delayNextWave);
            waveIndex++;
            //Safety iteration
            if (waveIndex >= maxWave)
            {
                waveIndex = 0;
            }
        }
    }
}
