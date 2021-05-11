using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public List<WaveMonsterController> waveMonsterControllers;

    private int numberMonster;
    WaitForSeconds delay;
    public float delayTime;
    // Start is called before the first frame update
    void Start()
    {
        delay = new WaitForSeconds(delayTime);
        numberMonster = waveMonsterControllers.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Setup()
    {

    }
    public void Run()
    {
        StartCoroutine(OnWaveRunning());
    }
    IEnumerator OnWaveRunning()
    {
        while(true)
        {
            var randomIndex = Random.Range(0, numberMonster);
            var wave = Lean.Pool.LeanPool.Spawn(waveMonsterControllers[randomIndex], this.transform);
            wave.Run();
            yield return delay;
        }
    }
}
