using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster03Controller : BaseMonster
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            Vector3 target = new Vector3(transform.position.x, -GameHelper.get_current_screenbound().y - 5.5f, 0f);
            transform.position = Vector3.MoveTowards(transform.position,
            Vector3.Lerp(transform.position, target, t_lerp), current_movespeed * Time.deltaTime);
            if (transform.position.y <= -GameHelper.get_current_screenbound().y - 5.0f)
            {

                isRunning = false;
                current_movespeed = base_movespeed;
                this.StopCoroutine(CheckDie());
                Lean.Pool.LeanPool.Despawn(this.gameObject);
            }
        }
    }
    public override void InitMonster()
    {
        base.InitMonster();
    }
    public override void Run()
    {
        InitMonster();
        isRunning = true;
        //GetComponent<Rigidbody2D>().velocity = new Vector2(0,current_movespeed);
        StartCoroutine(CheckDie());
        base.Run();
    }
    public override IEnumerator CheckDie()
    {
        yield return new WaitUntil(() => current_hp <= 0);
        GameManager.Instance.CreateDieFx(transform.position);
        GameManager.Instance.sfx.PlayOneShot(GameManager.Instance.monster_die_sfx, 0.3f);
        GameManager.Instance.camera_shake_fx.Shake();
        current_movespeed = base_movespeed;
        Lean.Pool.LeanPool.Despawn(this.gameObject);
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            UpdateHP(1);
        }
        else if(collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.GetComponent<BaseBullet>().Reset();
        }
    }
}
