using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    public int base_hp;
    public int current_hp;


    public float firerate;
    public int damage;

    public Lean.Pool.LeanGameObjectPool bullet_pool;
    public Transform fire_point;

    #region UI
    public UIHeartController heart_panel;
    #endregion


    #region SFX
    [Header("SFX")]
    public AudioSource sfx;
    public AudioClip coin_collect_sfx;
    #endregion
    private int counter;

    void Start()
    {
        StartShip();
    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// Init all parameters and start ship
    /// </summary>
    void StartShip()
    {
        counter = 0;
        current_hp = base_hp;
        StartCoroutine(Shoot());
        StartCoroutine(CheckDie());
    }
    void UpdateHP(int damage)
    {
        current_hp -= damage;
    }
    IEnumerator Shoot()
    {
        while(true)
        {
            var bullet = bullet_pool.Spawn(fire_point.position, Quaternion.identity);
            bullet.GetComponent<BaseBullet>().damage = damage;
            bullet.GetComponent<BaseBullet>().Shoot();
            yield return new WaitForSeconds(firerate);
        }
    }
    IEnumerator CheckDie()
    {
        yield return new WaitUntil(() => current_hp <= 0);
        GetComponent<SpaceShipMovement>().OnDisableTouch();
        this.gameObject.SetActive(false);
        StopAllCoroutines();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Ship is invincible state after hit the enemies
        if (counter > 1) return;

        if(collision.gameObject.tag =="Enemy")
        {
            GameManager.Instance.camera_shake_fx.Shake();
            GetComponent<Animator>().Play("ship_get_hit_anim");
            UpdateHP(1);
            heart_panel.UpdateHeartUI();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            GameManager.Instance.UpdateCoin(collision.gameObject.GetComponent<CoinController>().coin_value);
            sfx.PlayOneShot(coin_collect_sfx);
            Lean.Pool.LeanPool.Despawn(collision.gameObject);
        }
    }
    public void IncreaseCounterAnimator()
    {
        if (counter >= 5) 
        { 
            counter = 0;
            GetComponent<Animator>().SetInteger("Counter", counter);
            return;
        }
        counter = counter + 1;
        GetComponent<Animator>().SetInteger("Counter", counter);
    }
}
