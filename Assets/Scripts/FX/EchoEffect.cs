using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class EchoEffect : MonoBehaviour
{
    public GameObject prefab;
    public float spawnTime;
    public float despawnTime;
    public int originSortingLayerOrder;
    private int currentLayerOrder;
    private float timer;
    private bool isPlay;
    private void Start()
    {
        isPlay = false;
        currentLayerOrder = originSortingLayerOrder - 1;

    }
    public void PlayEchoEffect()
    {
        isPlay = true;
        currentLayerOrder = originSortingLayerOrder - 1;
    }
    public void StopEchoEffect()
    {
        isPlay = false;
        
    }

    private void Update()
    {
        if(isPlay)
        {
            if (timer >= spawnTime)
            {
                //Spawn echo object
                GameObject echoObj = LeanPool.Spawn(prefab, transform.position, Quaternion.identity);
                echoObj.GetComponent<SpriteRenderer>().sortingOrder = currentLayerOrder;
                currentLayerOrder -= 1;
                LeanPool.Despawn(echoObj, despawnTime);
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }  
    }
}
