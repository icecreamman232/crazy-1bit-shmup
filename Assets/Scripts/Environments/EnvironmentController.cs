using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentHolder
{
    public EnvironmentWithCustomPath environmentEntity;

    public float delay;
}

public class EnvironmentController : MonoBehaviour
{
    //Wrapper class to control how look environment design is
    public List<EnvironmentHolder> environmentEntityList;
    public int numberEntityList;
    public bool isWaveFinished;

    public int numberEntityCompleteDuty;

    private void OnEnable()
    {
        for (int i = 0; i < environmentEntityList.Count; i++)
        {
            environmentEntityList[i].environmentEntity.OnDestroy += OnCompleteWaveCycle;
            environmentEntityList[i].environmentEntity.OnFinishRun += OnCompleteWaveCycle;
        }
    }
    public void Run()
    {
        numberEntityCompleteDuty = 0;
        numberEntityList = environmentEntityList.Count;
        isWaveFinished = false;
        StartCoroutine(OnCreatingEntity());
    }
    public void Reset()
    {
        for (int i = 0; i < environmentEntityList.Count; i++)
        {
            //Unsubscribe events to prevent memory leak
            environmentEntityList[i].environmentEntity.OnDestroy -= OnCompleteWaveCycle;
            environmentEntityList[i].environmentEntity.OnFinishRun -= OnCompleteWaveCycle;

            environmentEntityList[i].environmentEntity.bezierMoveController.Stop();
        }
        Lean.Pool.LeanPool.Despawn(this.gameObject);
    }
    private IEnumerator OnCreatingEntity()
    {
        for (int i = 0; i < environmentEntityList.Count; i++)
        {
            environmentEntityList[i].environmentEntity.Setup();
            environmentEntityList[i].environmentEntity.Spawn();
            yield return new WaitForSeconds(environmentEntityList[i].delay);
        }
    }
    private void OnCompleteWaveCycle()
    {
        //Entity finish their run without getting hurt (died)
        numberEntityCompleteDuty++;
        //All entity complete their cycle
        if (numberEntityCompleteDuty >= environmentEntityList.Count)
        {
            isWaveFinished = true;
            Reset();
        }
    }
    #region Editor
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //9:16
        Gizmos.DrawWireCube(transform.position, new Vector3(2.9f * 2, 5.1f * 2));
        Gizmos.color = Color.blue;
        //9:18
        Gizmos.DrawWireCube(transform.position, new Vector3(2.9f * 2, 5.8f * 2));
    }


#endif
    #endregion
}
