using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ShipStatus
{
    NORMAL              = 0,            //Normal State

    /// <summary>
    /// - Không mất máu khi bị bắn/va chạm <br></br>
    /// - Vẫn có thể bắn đạn <br></br>
    /// - Vẫn có thể di chuyển <br></br>
    /// </summary>
    INVINCIBLE = 1,


    /// <summary>
    /// - Không mất máu khi bị bắn/va chạm <br></br>
    /// - Không thể bắn đạn <br></br>
    /// - Không thể di chuyển <br></br>
    /// </summary>
    DISABLE = 2,
    

    DEATH = 3,
}

public class SpaceShipController : MonoBehaviour
{
    #region Public field
    [Header("Gun")]
    public Gun gun;

    [Header("Basic Information")]
    public int baseHP;
    public int currentHP;
    public float invincibleDuration;

    public ShipStatus currentStatus;

    [Header("Reference")]
    public Lean.Pool.LeanGameObjectPool bulletPool;
    public ParticleSystem fireJetParticle;
    public SpriteRenderer shipSprite;
    public RankManager rankManager;
    #endregion

    #region UI
    [Header("UI")]
    public UIHealthBarController uiShipHPBar;
    #endregion

    #region SFX
    [Header("SFX")]
    public AudioSource sfx;
    public AudioClip sfxCoinCollect;
    #endregion

    
    private float timer;
    
    

    /// <summary>
    /// Init all parameters and start ship
    /// </summary>
    public void StartShip()
    {
        timer = 0;
        currentHP = baseHP;
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
        gun.Shoot();
    }
    public void StopShoot()
    {
        gun.Stop();
    }
    
    private IEnumerator CheckDie()
    {
        yield return new WaitUntil(() => currentHP <= 0);
        GetComponent<SpaceShipMovement>().OnDisableTouch();
        FXManager.Instance.CreateFX(fxID.DIE_SHIP_EXPLOSION, transform.position);
        shipSprite.enabled = false;
        fireJetParticle.Stop();
        gun.Stop();
        yield return new WaitForSeconds(1.5f);
        currentStatus = ShipStatus.DEATH;
        StopAllCoroutines();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Ship is invincible state after hit the enemies
        if (currentStatus == ShipStatus.INVINCIBLE)
        {
            return;
        }

        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            HandleGetHitByEntity(1);
        }
        if(collision.gameObject.layer == 11)
        {
            if (collision.gameObject.CompareTag("Meteor"))
            {
                HandleGetHitByEntity(1);
            }
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
