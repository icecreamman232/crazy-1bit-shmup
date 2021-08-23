using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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


    [SerializeField] private Vector3 screenBounds;

    private void OnEnable()
    {
        for (int i = 0; i < waveMonsterList.Count; i++)
        {
            //Add callback for getting detroyed event
            waveMonsterList[i].monster.OnDie += OnCompleteCycle;
            //Add callback for finishing path event
            waveMonsterList[i].monster.OnFinishRun += OnCompleteCycle;
        }
        screenBounds = GameHelper.HalfSizeOfCamera();
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
        Vector2 A = new Vector2();
        A.y = Camera.main.orthographicSize * 2;
        A.x = (Camera.main.aspect * Camera.main.orthographicSize) * 2;

        //Draw wireframe of screen ratio for better wave designing
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(A.x,A.y));
    }

    public void SnapToControlPoint()
    {
        for(int i = 0; i< waveMonsterList.Count; i++)
        {
            waveMonsterList[i].monster.transform.position = waveMonsterList[i].monster.IntroPath.GetPos(0);
        }
    }
#endif
    #endregion
}
