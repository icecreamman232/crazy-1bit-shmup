using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    
    public int coin_value;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init(int new_coin_value)
    {
        StopAllCoroutines();
        StartCoroutine(CheckCoinAlive());
        coin_value = new_coin_value;
    }
    /// <summary>
    /// Check if coin is out of screen or collected by ship
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckCoinAlive()
    {
        yield return new WaitUntil(() => transform.position.y <= -GameHelper.GetCurrentScreenBounds().y - 2.0f);
        Lean.Pool.LeanPool.Despawn(this.gameObject);
    }
}
