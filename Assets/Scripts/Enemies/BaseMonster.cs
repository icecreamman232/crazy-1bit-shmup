using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BaseMonster:  MonoBehaviour
{
    [Header("Monster Information")]
    /// <summary>
    /// Máu gốc của quái vật
    /// </summary>
    public int base_hp;
    /// <summary>
    /// Máu của quái vật khi được spawn ra tại wave cụ thể
    /// </summary>
    public int max_hp;
    public int current_hp;
    public float base_movespeed;
    public float current_movespeed;
    public int base_coin;
    public int base_coin_value;
    public int base_score;

    public bool isRunning;
    public Vector3 origin_position;
    protected float t_lerp = 0.1f;


    [Header("Reference Holders")]
    public GameObject hp_bar_ui;
    public GameObject coin_prefab;

    private void Start()
    {
       
    }
    public virtual void InitMonster()
    {
        max_hp = base_hp + base_hp * Mathf.RoundToInt(GameManager.Instance.endless_mode_data.hp_increase_per_wave
            * GameManager.Instance.GetCurrentLevelSpeed(GameManager.Instance.wave_index));
        current_hp = max_hp;
        current_movespeed = base_movespeed + GameManager.Instance.GetCurrentLevelSpeed(GameManager.Instance.wave_index) 
            * GameManager.Instance.endless_mode_data.speed_increase_per_wave;

        origin_position = transform.position;
        isRunning = false;
        

        hp_bar_ui.transform.localScale = new Vector3(1, 0.5f, 1);
        var hp_gameobject = hp_bar_ui.transform.parent;
        hp_gameobject.gameObject.SetActive(false);


        this.StopAllCoroutines();
    }

    public virtual void Run() 
    { 
        
    }

    public void UpdateHP(int _damage)
    {
        current_hp -= _damage;

        //Update HealthBar
        var percent_hp = (float)current_hp / max_hp;
        var last_scale = hp_bar_ui.transform.localScale;
        last_scale.x = percent_hp;
        hp_bar_ui.transform.localScale = last_scale;
    }
    public virtual  IEnumerator CheckDie()
    {       
        yield return new WaitUntil(() => current_hp <= 0);
        GameManager.Instance.UpdateScore(base_score);
        GameManager.Instance.CreateDieFx(transform.position);
        GameManager.Instance.sfx.PlayOneShot(GameManager.Instance.monster_die_sfx,0.3f);
        GameManager.Instance.camera_shake_fx.Shake();
        current_movespeed = base_movespeed;
        DropCoin();
        Lean.Pool.LeanPool.Despawn(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            UpdateHP(collision.gameObject.GetComponent<BaseBullet>().damage);
            if (!gameObject.transform.GetChild(0).gameObject.activeSelf) gameObject.transform.GetChild(0).gameObject.SetActive(true);

            collision.gameObject.GetComponent<BaseBullet>().Reset();
        }
    }

    void DropCoin()
    { 
        Vector3 trajectory = Random.insideUnitCircle * 100.0f;
        for (int i =0; i< base_coin; i++)
        {
            var coin = Lean.Pool.LeanPool.Spawn(coin_prefab, this.transform.position, Quaternion.identity);
            coin.GetComponent<CoinController>().Init(CoinValueBasedOnLevelSpeed());
            var force_vector = new Vector3(Random.Range(-100f, 100f) + trajectory.x, Random.Range(-100f, 300f) + trajectory.y, 0f);
            coin.GetComponent<Rigidbody2D>().AddForce(force_vector);
        }
    }
    int  CoinValueBasedOnLevelSpeed()
    {
        return Mathf.RoundToInt(base_coin_value * Mathf.RoundToInt(GameManager.Instance.endless_mode_data.coin_increase_per_wave
            * GameManager.Instance.GetCurrentLevelSpeed(GameManager.Instance.wave_index)));
    }
}
