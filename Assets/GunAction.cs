using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ID 1
public class GunAction : MonoBehaviour
{
    public System.Action<int> callback;
    public float counter;
    public Coroutine coroutine;
    public bool isPaused;
    public bool isRunning;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        isRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning)
        {
            if (isPaused) return;
            if (counter >= 5f)
            {
                Debug.Log("Gun fire");
                callback?.Invoke(1);
                counter = 0;
            }
            counter += Time.deltaTime;
        }
        

    }
    //public void Action()
    //{
    //    Debug.Log("Start Action 1");
    //    coroutine = StartCoroutine(MyAction());
    //}
    //private IEnumerator MyAction()
    //{
    //    while(true)
    //    {
    //        if(counter >=5f)
    //        {
    //            callback?.Invoke(1, coroutine);
    //            counter = 0;
    //            yield break;
    //        }
    //        counter += Time.deltaTime;
    //        yield return null;
    //    }
    //}
}
