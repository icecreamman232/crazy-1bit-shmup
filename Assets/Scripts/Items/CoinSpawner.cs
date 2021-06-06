using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;


    public void DropCoin(Vector3 spawnPos, int baseCoinValue, int baseNumberCoin)
    {
        Vector3 trajectory = Random.insideUnitCircle * 100.0f;
        for (int i = 0; i < baseNumberCoin; i++)
        {
            var coin = Lean.Pool.LeanPool.Spawn(coinPrefab, spawnPos, Quaternion.identity);
            coin.GetComponent<BaseCoin>().Init(CoinValueBasedOnLevelSpeed(baseCoinValue));
            var forceVector = new Vector3(
                Random.Range(-100f, 100f) + trajectory.x,
                Random.Range(-100f, 300f) + trajectory.y,
                0f);
            coin.GetComponent<Rigidbody2D>().AddForce(forceVector);
        }
    }
    public int CoinValueBasedOnLevelSpeed(int baseCoinValue)
    {
        return Mathf.RoundToInt(baseCoinValue
            * Mathf.RoundToInt(GameManager.Instance.endlessModeData.coinIncreasePerWave
            * GameManager.Instance.GetCurrentLevelSpeed()));
    }
}
