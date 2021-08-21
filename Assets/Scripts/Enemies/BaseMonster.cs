using System.Collections;
using UnityEngine;

public class BaseMonster : BaseEntity, IDamageable
{
    [SerializeField] protected int curHP;
    public int CurrentHP
    {
        get
        {
            return curHP;
        }
    }
    protected int maxHP;
    public int MaxHP
    {
        get
        {
            return maxHP;
        }
    }
    public MonsterDO data;
    /// <summary>
    /// Is interacting with an environment entity
    /// </summary>
    public bool isInteracting;

    //Damage cause if monster use its body to hit the other entities;
    public int baseImpactDamage;

    public System.Action OnDie;

    [Header("UI")]
    public UIMonsterHPBarController uiHealthBarController;

    [Header("Components")]
    public ItemSpawner itemSpawner;
    public CoinSpawner coinSpawner;


    #region Setup Methods

    public override void Setup()
    {
        gameObject.SetActive(true);
        isInteracting = false;
        //Data
        SetupHP();
        SetupMoveSpeed();
        //View
        uiHealthBarController.Setup();
    }
    private void SetupHP()
    {
        maxHP = data.baseHP + data.baseHP * Mathf.RoundToInt(GameManager.Instance.endlessModeData.HPIncreasePerWave
            * GameManager.Instance.GetCurrentLevelSpeed());
        curHP = maxHP;
    }

    private void SetupMoveSpeed()
    {
        currentMoveSpeed = data.baseMoveSpd + GameManager.Instance.GetCurrentLevelSpeed()
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
        if(!uiHealthBarController.IsShow)
        {
            uiHealthBarController.Show();
        }

        curHP -= damage;

        if (curHP <= 0)
        {
            GameManager.Instance.UpdateScore(data.baseScore);
            FXManager.Instance.CreateFX(fxID.DIE_MONSTER_EXPLOSION, transform.position);
            AudioManager.Instance.PlaySFX(AudioTagName.EXPLOSION,0.3f);
            //GameManager.Instance.cameraShakeFX.Shake();
            currentMoveSpeed = data.baseMoveSpd;
            var dropRate = ItemManager.Instance.GetRandomDropRate();
            if (dropRate <= itemSpawner.itemDropRate)
            {
                //coinSpawner.DropCoin(transform.position,data.itemData.baseCoinValue,data.itemData.baseNumCoin);
                //itemSpawner.DropItem(transform.position);
            }
            OnDie?.Invoke();
            gameObject.SetActive(false);
        }

        //Update Health Bar UI
        UpdateHPInterface();
    }
    protected void UpdateHPInterface()
    {
        var HPPercent = (float)curHP / maxHP;
        uiHealthBarController.UpdateHPBar(HPPercent);
    }

    #endregion Update Methods

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
    }

}