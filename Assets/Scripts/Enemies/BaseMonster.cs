using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BaseMonster:  BaseEntity
{
    [Header("Monster Stats")]
    public int baseNumberCoin;
    public int baseCoinValue;
    public int baseScore;
    //Damage cause if monster use its body to hit the other entities;
    public int baseImpactDamage;



    readonly protected float tLerp = 0.1f;

    public System.Action OnDie;

    [Header("Reference Holders")]
    public UIMonsterHPBarController uiHealthBarController;

    [Header("Components")]
    public ItemSpawner itemSpawner;
    public CoinSpawner coinSpawner;
    #region Setup Methods
    public override void Setup()
    {
        InitMonster();
        StartCoroutine(CheckDie());
    }
    public void InitMonster()
    {
        gameObject.SetActive(true);

        //Data
        SetupHP();
        SetupMoveSpeed();
        //View
        uiHealthBarController.Setup();
        StopAllCoroutines();
    }
    private void SetupHP()
    {
        maxHP = baseHP + baseHP * Mathf.RoundToInt(GameManager.Instance.endlessModeData.HPIncreasePerWave
            * GameManager.Instance.GetCurrentLevelSpeed());
        currentHP = maxHP;
    }
    private void SetupMoveSpeed()
    {
        currentMoveSpeed = baseMoveSpeed + GameManager.Instance.GetCurrentLevelSpeed()
                    * GameManager.Instance.endlessModeData.speedIncreasePerWave;
    }
    #endregion

    #region Spawn Methods
    public override void Spawn()
    {
 
    }
    #endregion

    #region Update Methods
    public void UpdateHP(int damage)
    {
        //Show the HP Bar
        if (!gameObject.transform.GetChild(0).gameObject.activeSelf)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        currentHP -= damage;
        //Update Health Bar UI
        var HPPercent = (float)currentHP / maxHP;
        uiHealthBarController.UpdateHPBar(HPPercent);

    }
    #endregion

    #region Collision Handling
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            HandleCollisionWithBullet(collision);
        }

    }
    public void HandleCollisionWithBullet(Collider2D collision)
    {
        UpdateHP(collision.gameObject.GetComponent<BaseBullet>().Damage);

        collision.gameObject.GetComponent<BaseBullet>().Reset();
    }
    #endregion

    public virtual  IEnumerator CheckDie()
    {       
        yield return new WaitUntil(() => currentHP <= 0);
        
        GameManager.Instance.UpdateScore(baseScore);
        FXManager.Instance.CreateFX(1,transform.position);
        GameManager.Instance.sfx.PlayOneShot(GameManager.Instance.monster_die_sfx,0.3f);
        //GameManager.Instance.cameraShakeFX.Shake();
        currentMoveSpeed = baseMoveSpeed;
        var dropRate = ItemManager.Instance.GetRandomDropRate();
        if(dropRate <= itemSpawner.itemDropRate)
        {
            //coinSpawner.DropCoin(transform.position,baseCoinValue,baseNumberCoin);
            //itemSpawner.DropItem(transform.position);
        }
        OnDie?.Invoke();
        gameObject.SetActive(false);
    }
  
    
}
