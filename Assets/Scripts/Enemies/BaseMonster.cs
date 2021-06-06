using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BaseMonster:  MonoBehaviour
{
    [Header("Monster Information")]
    public int baseHP;
    public int maxHP;
    public int currentHP;
    public float baseMoveSpeed;
    public float currentMoveSpeed;
    public int baseNumberCoin;
    public int baseCoinValue;
    public int baseScore;

    readonly protected float tLerp = 0.1f;

    public System.Action OnDie;

    [Header("Reference Holders")]
    public GameObject uiHPBar;

    [Header("Components")]
    public ItemSpawner itemSpawner;
    public CoinSpawner coinSpawner;
    private void Start()
    {

    }
    public virtual void InitMonster()
    {
        gameObject.SetActive(true);
        SetupHP();
        SetupUIHealthBar();
        SetupMoveSpeed();
        StopAllCoroutines();
    }
    public virtual void SetupHP()
    {
        maxHP = baseHP + baseHP * Mathf.RoundToInt(GameManager.Instance.endlessModeData.HPIncreasePerWave
            * GameManager.Instance.GetCurrentLevelSpeed());
        currentHP = maxHP;
    }
    private void SetupUIHealthBar()
    {
        uiHPBar.transform.localScale = new Vector3(1, 0.5f, 1);
        var hp_gameobject = uiHPBar.transform.parent;
        hp_gameobject.gameObject.SetActive(false);
    }
    public virtual void SetupMoveSpeed()
    {
        currentMoveSpeed = baseMoveSpeed + GameManager.Instance.GetCurrentLevelSpeed()
                    * GameManager.Instance.endlessModeData.speedIncreasePerWave;
    }

    public virtual void Setup() 
    {
        InitMonster();
        StartCoroutine(CheckDie());
    }

    public void UpdateHP(int damage)
    {
        currentHP -= damage;

        //Update Health Bar UI
        var HPPercent = (float)currentHP / maxHP;
        var lastScale = uiHPBar.transform.localScale;
        lastScale.x = HPPercent;
        uiHPBar.transform.localScale = lastScale;
    }
    public virtual  IEnumerator CheckDie()
    {       
        yield return new WaitUntil(() => currentHP <= 0);
        
        GameManager.Instance.UpdateScore(baseScore);
        GameManager.Instance.CreateDieFx(transform.position);
        GameManager.Instance.sfx.PlayOneShot(GameManager.Instance.monster_die_sfx,0.3f);
        GameManager.Instance.cameraShakeFX.Shake();
        currentMoveSpeed = baseMoveSpeed;
        var dropRate = ItemManager.Instance.GetRandomDropRate();
        if(dropRate <= itemSpawner.itemDropRate)
        {
            coinSpawner.DropCoin(transform.position,baseCoinValue,baseNumberCoin);
            itemSpawner.DropItem(transform.position);
        }
        OnDie?.Invoke();
        gameObject.SetActive(false);
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            UpdateHP(collision.gameObject.GetComponent<BaseBullet>().Damage);
            if (!gameObject.transform.GetChild(0).gameObject.activeSelf)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            collision.gameObject.GetComponent<BaseBullet>().Reset();
        }
    }

    //private void DropCoin()
    //{ 
    //    Vector3 trajectory = Random.insideUnitCircle * 100.0f;
    //    for (int i =0; i< baseNumberCoin; i++)
    //    {
    //        var coin = Lean.Pool.LeanPool.Spawn(coinPrefab, this.transform.position, Quaternion.identity);
    //        coin.GetComponent<CoinController>().Init(CoinValueBasedOnLevelSpeed());
    //        var forceVector = new Vector3(
    //            Random.Range(-100f, 100f) + trajectory.x, 
    //            Random.Range(-100f, 300f) + trajectory.y, 
    //            0f);
    //        coin.GetComponent<Rigidbody2D>().AddForce(forceVector);
    //    }
    //}

    //private int  CoinValueBasedOnLevelSpeed()
    //{
    //    return Mathf.RoundToInt(baseCoinValue 
    //        * Mathf.RoundToInt(GameManager.Instance.endlessModeData.coinIncreasePerWave
    //        * GameManager.Instance.GetCurrentLevelSpeed()));
    //}
}
