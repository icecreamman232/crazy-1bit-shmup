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
    public GameObject hp_bar_ui;
    public GameObject coin_prefab;
    public GameObject item_prefab;
    public List<ItemType> list_dropable_items;
    private int max_item;
    /// <summary>
    /// Value từ 0 ->1000
    /// </summary>
    public int item_drop_rate;



    private void Start()
    {
        max_item = list_dropable_items.Count;
    }
    public virtual void InitMonster()
    {
        this.gameObject.SetActive(true);
        SetupHP();
        SetupUIHealthBar();
        SetupMoveSpeed();
        StopAllCoroutines();
    }
    public virtual void SetupHP()
    {
        maxHP = baseHP + baseHP * Mathf.RoundToInt(GameManager.Instance.endless_mode_data.hp_increase_per_wave
            * GameManager.Instance.GetCurrentLevelSpeed());
        currentHP = maxHP;
    }
    private void SetupUIHealthBar()
    {
        hp_bar_ui.transform.localScale = new Vector3(1, 0.5f, 1);
        var hp_gameobject = hp_bar_ui.transform.parent;
        hp_gameobject.gameObject.SetActive(false);
    }
    public virtual void SetupMoveSpeed()
    {
        currentMoveSpeed = baseMoveSpeed + GameManager.Instance.GetCurrentLevelSpeed()
                    * GameManager.Instance.endless_mode_data.speed_increase_per_wave;
    }

    public virtual void Setup() 
    {
        InitMonster();
        StartCoroutine(CheckDie());
    }

    public void UpdateHP(int _damage)
    {
        currentHP -= _damage;

        //Update Health Bar UI
        var percent_hp = (float)currentHP / maxHP;
        var last_scale = hp_bar_ui.transform.localScale;
        last_scale.x = percent_hp;
        hp_bar_ui.transform.localScale = last_scale;
    }
    public virtual  IEnumerator CheckDie()
    {       
        yield return new WaitUntil(() => currentHP <= 0);
        
        GameManager.Instance.UpdateScore(baseScore);
        GameManager.Instance.CreateDieFx(transform.position);
        GameManager.Instance.sfx.PlayOneShot(GameManager.Instance.monster_die_sfx,0.3f);
        GameManager.Instance.camera_shake_fx.Shake();
        currentMoveSpeed = baseMoveSpeed;
        var drop_rate = ItemManager.Instance.GetRandomDropRate();
        if(drop_rate<=item_drop_rate)
        {
            //DropCoin();
            DropItem();
        }
        OnDie?.Invoke();
        this.gameObject.SetActive(false);
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            UpdateHP(collision.gameObject.GetComponent<BaseBullet>().Damage);
            if (!gameObject.transform.GetChild(0).gameObject.activeSelf) gameObject.transform.GetChild(0).gameObject.SetActive(true);
            collision.gameObject.GetComponent<BaseBullet>().Reset();
        }
    }

    void DropCoin()
    { 
        Vector3 trajectory = Random.insideUnitCircle * 100.0f;
        for (int i =0; i< baseNumberCoin; i++)
        {
            var coin = Lean.Pool.LeanPool.Spawn(coin_prefab, this.transform.position, Quaternion.identity);
            coin.GetComponent<CoinController>().Init(CoinValueBasedOnLevelSpeed());
            var force_vector = new Vector3(Random.Range(-100f, 100f) + trajectory.x, Random.Range(-100f, 300f) + trajectory.y, 0f);
            coin.GetComponent<Rigidbody2D>().AddForce(force_vector);
        }
    }
    void DropItem()
    {
        Vector3 trajectory = Random.insideUnitCircle * 100.0f;
        var item = Lean.Pool.LeanPool.Spawn(item_prefab, transform.position, Quaternion.identity);
        var item_type_index = Random.Range(0, max_item);
        item.GetComponent<BaseItem>().SetupItem(list_dropable_items[item_type_index]);
        var force_vector = new Vector3(Random.Range(-100f, 100f) + trajectory.x, Random.Range(-100f, 300f) + trajectory.y, 0f);
        item.GetComponent<Rigidbody2D>().AddForce(force_vector); 
    }
    int  CoinValueBasedOnLevelSpeed()
    {
        return Mathf.RoundToInt(baseCoinValue * Mathf.RoundToInt(GameManager.Instance.endless_mode_data.coin_increase_per_wave
            * GameManager.Instance.GetCurrentLevelSpeed()));
    }
}
