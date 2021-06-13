using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ShipStatus
{
    NORMAL              = 0,            //Normal State

    /*
     * - Không mất máu khi bị bắn/va chạm
     * - Vẫn có thể bắn đạn
     * - Vẫn có thể di chuyển
     */
    INVINCIBLE = 1,


    /*
     * - Không mất máu khi bị bắn/va chạm
     * - Không thể bắn đạn
     * - Không thể di chuyển
     */
    DISABLE = 2, 
}

public class SpaceShipController : MonoBehaviour
{
    [Header("Basic Information")]
    public int baseHP;
    public int currentHP;
    public float firerate;
    public int damage;

    public ShipStatus currentStatus;


    public Lean.Pool.LeanGameObjectPool bulletPool;
    public Transform firePoint;
    public ParticleSystem fireJetParticle;
    public ParticleSystem shipExplosionFX;

    #region UI
    public UIHealthBarController uiShipHPBar;
    #endregion

    #region SFX
    [Header("SFX")]
    public AudioSource sfx;
    public AudioClip sfxCoinCollect;
    #endregion

    public SpriteRenderer shipSprite;
    private float timer;
    public float invincibleDuration;
    public bool isDied;
    public RankManager rankManager;

    private Coroutine shootingCoroutine;

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
        timer = 0;
        currentHP = baseHP;
        isDied = false;
        shipSprite.enabled = true;
        currentStatus = ShipStatus.NORMAL;


        //Reset alpha for some case that ship get died so fast cause alpha controlled by shader disrupted
        Color c = shipSprite.color;
        c.a = 1.0f;
        shipSprite.color = c;

        fireJetParticle.Play();
        GetComponent<SpaceShipMovement>().SetShipPosition();
        GetComponent<Animator>().Play("ship_idle");
       
        StartCoroutine(CheckDie());
    }
    private void UpdateHP(int damage)
    {
        currentHP -= damage;
    }
    public void BeginShoot()
    {
        shootingCoroutine = StartCoroutine(Shoot());
    }
    public void StopShoot()
    {
        StopCoroutine(shootingCoroutine);
    }
    private IEnumerator Shoot()
    {
        WaitForSeconds firerateDelay = new WaitForSeconds(firerate);
        while(currentHP > 0)
        {
            if(currentHP <=0)
            {
                yield break;
            }
            var bullet = bulletPool.Spawn(firePoint.position, Quaternion.identity);
            bullet.GetComponent<BaseBullet>().Damage = damage;
            bullet.GetComponent<BaseBullet>().Shoot();
            yield return firerateDelay;
        }
    }
    private IEnumerator CheckDie()
    {
        yield return new WaitUntil(() => currentHP <= 0);
        GetComponent<SpaceShipMovement>().OnDisableTouch();
        shipExplosionFX.Play();
        shipSprite.enabled = false;
        fireJetParticle.Stop();
        yield return new WaitForSeconds(1.5f);
        isDied = true;
        StopAllCoroutines();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Ship is invincible state after hit the enemies
        if (currentStatus == ShipStatus.INVINCIBLE)
        {
            Debug.Log("Here ignore");
            return;
        }

        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            HandleGetHitByEntity(1);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            HandleCollectCoin(collision);
        }
        if (collision.gameObject.CompareTag("Item"))
        {
            HandleCollectItem(collision);
        }
    }
    private IEnumerator OnInvincible()
    {
        WaitForSeconds delay = new WaitForSeconds(.15f);
        while(currentHP > 0)
        {
            if (timer >= invincibleDuration)
            {
                timer = 0;
                shipSprite.color = new Color(1, 1, 1, 1);
                currentStatus = ShipStatus.NORMAL;
                yield break;
            }
            yield return delay;
            Color c = shipSprite.color;
            float saveAlpha = c.a;
            c.a = 0f;
            shipSprite.color = c;
            yield return delay;
            c.a = saveAlpha;
            shipSprite.color = c;
            timer += Time.deltaTime + 0.3f;
        }       
    }

    public void HandleGetHitByEntity(int damage)
    {
        GameManager.Instance.cameraShakeFX.Shake();
        currentStatus = ShipStatus.INVINCIBLE;
        StartCoroutine(OnInvincible());
        UpdateHP(damage);
        uiShipHPBar.UpdateHealthBarUI();
    }

    private void HandleCollectCoin(Collision2D coin)
    {
        GameManager.Instance.UpdateCoin(coin.gameObject.GetComponent<BaseCoin>().coinValue);
        sfx.PlayOneShot(sfxCoinCollect);
        Lean.Pool.LeanPool.Despawn(coin.gameObject);
    }
    private void HandleCollectItem(Collision2D item)
    {
        DataManager.Instance.SaveDataToLocalStorage();
        rankManager.UpdateRankPoints(item.gameObject.GetComponent<BaseItem>().m_rank_point);
        sfx.PlayOneShot(sfxCoinCollect);
        Lean.Pool.LeanPool.Despawn(item.gameObject);
    }
}
