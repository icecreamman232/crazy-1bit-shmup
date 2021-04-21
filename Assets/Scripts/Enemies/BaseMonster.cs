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
    private void Start()
    {
       
    }
    public virtual void InitMonster()
    {
        current_hp = base_hp;
        origin_position = transform.position;
        isRunning = false;
    }

    public virtual void Run() { }

    public void UpdateHP(int _damage)
    {
        current_hp -= _damage;
    }
    protected IEnumerator CheckDie()
    {
        yield return new WaitUntil(() => current_hp <= 0);
        this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            UpdateHP(collision.gameObject.GetComponent<BaseBullet>().damage);
            collision.gameObject.GetComponent<BaseBullet>().Reset();
            Debug.Log("Bullet hit mons");
        }
    }
}
