using System.Collections;
using UnityEngine;
using System;
using Lean.Pool;

public enum ShipStatus
{
    NORMAL = 0,            //Normal State

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

public class SpaceShipController : MonoBehaviour, IDamageable
{
    #region Public field

    [Header("Gun")]
    public Gun gun;

    [Header("Basic Information")]
    public ShipStatus currentStatus;
    [SerializeField]
    private int baseHP;
    [SerializeField]
    private int currentHP;
    public int CurrentHP
    {
        get 
        { 
            return currentHP; 
        }
        set 
        { 
            if(value < 0)
            {
                currentHP = 0;
            }
            else
            {
                currentHP = value;
            }
        }
    }

    [SerializeField]
    private float invincibleDuration;

    [Header("Reference")]
    public ParticleSystem fireJetParticle;
    public SpriteRenderer shipSprite;
    public RankManager rankManager;

    private SpaceShipMovement movementController;
    private GameManager gameManager;
    #endregion Public field

    #region UI

    [Header("UI")]
    public UIHealthBarController uiShipHPBar;

    #endregion UI

    #region SFX

    [Header("SFX")]
    public AudioSource sfx;

    public AudioClip sfxCoinCollect;

    #endregion SFX

    private float timer;

    public event Action<ShipStatus> OnDeath;

    private void Start()
    {
        movementController = GetComponent<SpaceShipMovement>();
        gameManager = GameManager.Instance;
    }

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
        movementController.SetShipPosition();
        GetComponent<Animator>().Play("ship_idle");

        
    }
    public void TakeDamage(int damage)
    {
        //Debug.Log("Ship took " + damage + " dmg");
    }
    private void UpdateHP(int damage)
    {
        currentHP -= damage;
        if(currentHP <= 0)
        {
            movementController.OnDisableTouch();
            FXManager.Instance.CreateFX(fxID.DIE_SHIP_EXPLOSION, transform.position);
            shipSprite.enabled = false;
            fireJetParticle.Stop();
            StopShoot();                  
            StartCoroutine(ShortDelayBeforeNotifyDeath());
        }
    }
    private IEnumerator ShortDelayBeforeNotifyDeath()
    {
        yield return new WaitForSeconds(1.0f);
        OnDeath?.Invoke(currentStatus);
        currentStatus = ShipStatus.DEATH;   
    }
    public void BeginShoot()
    {
        gun.Shoot();
    }

    public void StopShoot()
    {
        gun.Stop();
    }

    #region Collison Detection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Ship is invincible state after hit the enemies
        if (currentStatus == ShipStatus.INVINCIBLE)
        {
            return;
        }

        if(!collision.gameObject.CompareTag("Bullet"))
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
        while (currentHP > 0)
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
        gameManager.cameraShakeFX.Shake();
        currentStatus = ShipStatus.INVINCIBLE;
        StartCoroutine(OnInvincible());
        TakeDamage(damage);
        UpdateHP(damage);
        uiShipHPBar.UpdateHealthBarUI();
    }

    private void HandleCollectCoin(Collision2D coin)
    {
        gameManager.UpdateCoin(coin.gameObject.GetComponent<BaseCoin>().coinValue);
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

    
    #endregion Collison Detection
}