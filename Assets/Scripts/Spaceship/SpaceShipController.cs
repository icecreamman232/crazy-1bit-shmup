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
    public ParticleSystem fire_jet_particle;
    public ParticleSystem ship_explosion_fx;

    #region UI
    public UIHealthBarController ui_ship_health_bar;
    #endregion


    #region SFX
    [Header("SFX")]
    public AudioSource sfx;
    public AudioClip coin_collect_sfx;
    #endregion


    public SpriteRenderer ship_sprite;
    public float time_counter;
    public float invincible_duration;

    public bool isDied;

    public RankManager rank_manager;


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
        isDied = false;
        ship_sprite.enabled = true;

        //Reset alpha for some case that ship get died so fast cause alpha controlled by shader disrupted
        Color c = ship_sprite.color;
        c.a = 1.0f;
        ship_sprite.color = c;

        fire_jet_particle.Play();
        GetComponent<SpaceShipMovement>().SetShipPosition();
        GetComponent<Animator>().Play("ship_idle");
       
        StartCoroutine(CheckDie());
    }
    void UpdateHP(int damage)
    {
        current_hp -= damage;
    }
    public void BeginShoot()
    {
       // StartCoroutine(Shoot());
    }
    IEnumerator Shoot()
    {
        WaitForSeconds fire_rate_delay = new WaitForSeconds(firerate);
        while(current_hp > 0)
        {
            if(current_hp <=0)
            {
                yield break;
            }
            var bullet = bullet_pool.Spawn(fire_point.position, Quaternion.identity);
            bullet.GetComponent<BaseBullet>().Damage = damage;
            bullet.GetComponent<BaseBullet>().Shoot();
            yield return fire_rate_delay;
        }
    }
    IEnumerator CheckDie()
    {
        yield return new WaitUntil(() => current_hp <= 0);
        GetComponent<SpaceShipMovement>().OnDisableTouch();
        ship_explosion_fx.Play();
        ship_sprite.enabled = false;
        fire_jet_particle.Stop();
        yield return new WaitForSeconds(1.5f);
        isDied = true;
        StopAllCoroutines();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Ship is invincible state after hit the enemies
        if (time_counter > 0) return;

        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            GameManager.Instance.camera_shake_fx.Shake();
            StartCoroutine(OnInvincible());
            UpdateHP(1);
            ui_ship_health_bar.UpdateHealthBarUI();
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
        if (collision.gameObject.CompareTag("Item"))
        {
            DataManager.Instance.SaveDataToLocalStorage();
            rank_manager.UpdateRankPoints(collision.gameObject.GetComponent<BaseItem>().m_rank_point);
            sfx.PlayOneShot(coin_collect_sfx);
            Lean.Pool.LeanPool.Despawn(collision.gameObject);
        }
    }
    IEnumerator OnInvincible()
    {
        WaitForSeconds delay = new WaitForSeconds(.15f);
        while(current_hp > 0)
        {
            if (time_counter >= invincible_duration)
            {
                time_counter = 0;
                ship_sprite.color = new Color(1, 1, 1, 1);
                yield break;
            }
            yield return delay;
            Color c = ship_sprite.color;
            float save_alpha = c.a;
            c.a = 0f;
            ship_sprite.color = c;
            yield return delay;
            c.a = save_alpha;
            ship_sprite.color = c;
            time_counter += Time.deltaTime + 0.3f;
        }
        
    }
}
