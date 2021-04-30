using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster03Controller : BaseMonster
{
    public GameObject alert_sign;
    public Animator sign_animator;
    WaitForSeconds alert_duration;


    // Start is called before the first frame update
    void Start()
    {
        alert_duration = new WaitForSeconds(2.0f);
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
        sign_animator.Play("alert_sign_anim");
        StartCoroutine(WaitForEndAlert());
    }
    IEnumerator WaitForEndAlert()
    {
        yield return alert_duration;
        sign_animator.Play("New State");
        alert_sign.SetActive(false);
        isRunning = true;
        StartCoroutine(CheckDie());
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
