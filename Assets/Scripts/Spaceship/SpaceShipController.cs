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


    public SpriteRenderer ship_sprite;
    float time_counter;
    public float invincible_duration;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Init all parameters and start ship
    /// </summary>
    public void StartShip()
    {
        time_counter = 0;
        current_hp = base_hp;
        GetComponent<SpaceShipMovement>().OnEnableTouch();
        GetComponent<SpaceShipMovement>().SetShipPosition();
        this.gameObject.SetActive(true);
        GetComponent<Animator>().Play("ship_idle");
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
        if (time_counter > 0) return;

        if(collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.camera_shake_fx.Shake();
            StartCoroutine(OnInvincible());
            UpdateHP(1);
            heart_panel.UpdateHeartUI();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            GameManager.Instance.UpdateCoin(collision.gameObject.GetComponent<CoinController>().coin_value);
            sfx.PlayOneShot(coin_collect_sfx);
            Lean.Pool.LeanPool.Despawn(collision.gameObject);
        }
    }
    IEnumerator OnInvincible()
    {
        while(true)
        {
            if (time_counter >= invincible_duration)
            {
                time_counter = 0;
                yield break;
            }
            yield return new WaitForSeconds(.15f);
            Color c = ship_sprite.color;
            float save_alpha = c.a;
            c.a = 0f;
            ship_sprite.color = c;
            yield return new WaitForSeconds(.15f);
            c.a = save_alpha;
            ship_sprite.color = c;
            time_counter += Time.deltaTime + 0.3f;
        }
        
    }
}
