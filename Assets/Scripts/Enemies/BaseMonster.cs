using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;


public class BaseMonster:  MonoBehaviour
{

    public bool isRunning;
    public Vector3 origin_position;
    public float t_lerp = 0.1f;
    public  int base_hp;
    /// <summary>
    /// Máu hiện tại của quái
    /// </summary>
    public  int current_hp;

    public float base_movespeed;
    public int base_score;
    public int base_coin;
    public GameObject coin_prefab;

    private void Start()
    {
       
    }
    public virtual void InitMonster()
    {
        current_hp = base_hp;
        origin_position = transform.position;
        isRunning = false;
        this.StopAllCoroutines();
    }

    public virtual void Run(float speed) 
    { 
        
    }

    public void UpdateHP(int _damage)
    {
        current_hp -= _damage;
    }
    protected IEnumerator CheckDie()
    {
        
        yield return new WaitUntil(() => current_hp <= 0);
        GameManager.Instance.UpdateScore(base_score);
        GameManager.Instance.CreateDieFx(transform.position);
        GameManager.Instance.camera_shake_fx.Shake();
        DropCoin();
        Lean.Pool.LeanPool.Despawn(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            UpdateHP(collision.gameObject.GetComponent<BaseBullet>().damage);
            collision.gameObject.GetComponent<BaseBullet>().Reset();
        }
    }
    void DropCoin()
    {
        Vector3 trajectory = Random.insideUnitCircle * 100.0f;
        for (int i =0; i< base_coin; i++)
        {
            var coin = Lean.Pool.LeanPool.Spawn(coin_prefab, this.transform.position, Quaternion.identity);
            coin.GetComponent<CoinController>().Init();
            var force_vector = new Vector3(Random.Range(-100f, 100f) + trajectory.x, Random.Range(-100f, 300f) + trajectory.y, 0f);
            coin.GetComponent<Rigidbody2D>().AddForce(force_vector);


        }
    }
}
