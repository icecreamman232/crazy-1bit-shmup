using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCoin : MonoBehaviour
{
    public int coinValue;
    private float screenBoundY;
    // Start is called before the first frame update
    void Start()
    {
        screenBoundY = GameHelper.GetCurrentScreenBounds().y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init(int newCoinValue)
    {
        StopAllCoroutines();
        StartCoroutine(CheckCoinAlive());
        coinValue = newCoinValue;
    }
    IEnumerator CheckCoinAlive()
    {
        //Check if coin is out of screen
        yield return new WaitUntil(() => transform.position.y <= -screenBoundY - 2.0f);
        Lean.Pool.LeanPool.Despawn(this.gameObject);
    }
}
