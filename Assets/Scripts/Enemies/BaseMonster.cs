using System.Collections;
using UnityEngine;

public class BaseMonster : BaseEntity, IDamageable
{
    [Header("Monster Stats")]
    //Is interacting with an environment entity
    public bool isInteracting;

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
    }

    public void InitMonster()
    {
        gameObject.SetActive(true);
        isInteracting = false;
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

    #endregion Setup Methods

    #region Spawn Methods

    public override void Spawn()
    {
    }

    #endregion Spawn Methods

    #region Update Methods
    public void TakeDamage(int damage)
    {
        //Show the HP Bar
        if (!gameObject.transform.GetChild(0).gameObject.activeSelf)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        currentHP -= damage;

        if (currentHP <= 0)
        {
            GameManager.Instance.UpdateScore(baseScore);
            FXManager.Instance.CreateFX(fxID.DIE_MONSTER_EXPLOSION, transform.position);
            GameManager.Instance.sfx.PlayOneShot(GameManager.Instance.monster_die_sfx, 0.3f);
            //GameManager.Instance.cameraShakeFX.Shake();
            currentMoveSpeed = baseMoveSpeed;
            var dropRate = ItemManager.Instance.GetRandomDropRate();
            if (dropRate <= itemSpawner.itemDropRate)
            {
                //coinSpawner.DropCoin(transform.position,baseCoinValue,baseNumberCoin);
                //itemSpawner.DropItem(transform.position);
            }
            OnDie?.Invoke();
            gameObject.SetActive(false);
        }

        //Update Health Bar UI
        UpdateHPInterface();
    }
    public void UpdateHPInterface()
    {
        var HPPercent = (float)currentHP / maxHP;
        uiHealthBarController.UpdateHPBar(HPPercent);
    }

    #endregion Update Methods

    #region Collision Handling

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameHelper.IsInsideScreenBounds(transform.position))
        {
        }
    }

    

    #endregion Collision Handling
}