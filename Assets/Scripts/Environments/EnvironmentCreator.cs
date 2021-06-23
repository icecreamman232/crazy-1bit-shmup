using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCreator : MonoBehaviour
{
    public EnvironmentStageDO environmentDesign;
    public bool isRunning;
    public EnvironmentController currentEnvironmentEntity;
    public int currentIndex;
    private int maxWave;
    public void Setup()
    {
        currentIndex = 0;
        maxWave = environmentDesign.listEnvironment.Count;
        currentEnvironmentEntity = null;
        isRunning = false;
             
    }
    public void Run()
    {        
        isRunning = true;
        StartCoroutine(OnRunning());
    }
    public void Reset()
    {
        isRunning = false;
        StopCoroutine(OnRunning());
        currentEnvironmentEntity.isWaveFinished = true;
        currentEnvironmentEntity.Reset();
    }
    private IEnumerator OnRunning()
    {
        while(true)
        {
            if(!isRunning)
            {
                yield break;
            }
            currentEnvironmentEntity = Lean.Pool.LeanPool.Spawn(
                environmentDesign.listEnvironment[currentIndex].environment,transform);
            currentEnvironmentEntity.Run();
            yield return new WaitUntil(()=> currentEnvironmentEntity.isWaveFinished);
            yield return new WaitForSeconds(environmentDesign.listEnvironment[currentIndex].delayTime);
            currentIndex++;
            if(currentIndex >= maxWave)
            {
                currentIndex = 0;
            }
        }
    }
}
