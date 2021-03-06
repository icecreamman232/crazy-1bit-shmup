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
    #region Basic Stat
    [Header("Basic Stats")]
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
            if (value < 0)
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
    #endregion

    #region Weapons
    [Header("Weapons")]
    public Gun gun;
    #endregion

    [Header("Reference")]
    public ParticleSystem fireJetParticle;
    public SpriteRenderer shipSprite;
    public RankManager rankManager;

    private GameManager gameManager;

    private float shipSpriteWidth;
    private float shipSpriteHeight;
    private float lastPosX;
    private Animator shipAnimator;

    private Vector2 screenBounds;

    private float timer;

    public event Action<ShipStatus> OnDeath;


    private int lastAnimHash;
    private int idleAnimHash;
    private int turnLeftHash;
    private int turnRightHash;


    #region PC Controller
    private Vector3 moveDir;
    public float moveSpd;
    #endregion

    public Action TakeDamageAction;

    private void Start()
    {
        gameManager = GameManager.Instance;
        shipAnimator = GetComponent<Animator>();
        screenBounds = GameHelper.HalfSizeOfCamera();

        shipSpriteWidth = shipSprite.bounds.size.x;
        shipSpriteHeight = shipSprite.bounds.size.y;

        idleAnimHash = Animator.StringToHash("ship_idle");
        turnLeftHash = Animator.StringToHash("ship_turn_left_anim");
        turnRightHash = Animator.StringToHash("ship_turn_right_anim");
        lastAnimHash = 0;
        moveDir = Vector3.zero;
   
    }
    #region Ship Controls
    public void ShipMoveLeft()
    {
        moveDir.x = -1;
    }
    public void ShipMoveRight()
    {
        moveDir.x = 1;
    }
    public void ShipMoveUp()
    {
        moveDir.y = 1;
    }
    public void ShipMoveDown()
    {
        moveDir.y = -1;
    }
    public void ShipStay()
    {
        moveDir = Vector3.zero;
    }
    public void Shoot()
    {
        gun.Shoot();
    }
    #endregion
    private void Update()
    {
        if (currentStatus == ShipStatus.NORMAL || currentStatus == ShipStatus.INVINCIBLE)
        {
            
            transform.Translate(moveDir * moveSpd * Time.deltaTime);

            var curPos = transform.position;

            if (curPos.x <= -screenBounds.x + shipSpriteWidth * 0.5f) curPos.x = -screenBounds.x + shipSpriteWidth * 0.5f;
            if (curPos.x >= screenBounds.x - shipSpriteWidth * 0.5f) curPos.x = screenBounds.x - shipSpriteWidth * 0.5f;
            if (curPos.y <= -screenBounds.y + shipSpriteWidth * 0.5f) curPos.y = -screenBounds.y + shipSpriteWidth * 0.5f;
            if (curPos.y >= screenBounds.y - shipSpriteWidth * 0.5f) curPos.y = screenBounds.y - shipSpriteWidth * 0.5f;

            transform.position = curPos;

            //Ship keep turning left
            if (transform.position.x < lastPosX)
            {
                if(lastAnimHash != turnLeftHash)
                {
                    shipAnimator.Play(turnLeftHash);
                    lastAnimHash = turnLeftHash;
                }              
            }
            //Ship keep turning right
            else if (transform.position.x > lastPosX)
            {
                if(lastAnimHash!= turnRightHash)
                {
                    shipAnimator.Play(turnRightHash);
                    lastAnimHash = turnRightHash;
                }            
            }
            else
            {
                if(lastAnimHash!=idleAnimHash)
                {
                    shipAnimator.Play(idleAnimHash);
                    lastAnimHash = idleAnimHash;
                }              
            }

            lastPosX = transform.position.x;
        }
    }
    /// <summary>
    /// Init all parameters and start ship
    /// </summary>
    public void StartShip()
    {
        lastAnimHash = 0;
        timer = 0;
        currentHP = baseHP;
        shipSprite.enabled = true;
        currentStatus = ShipStatus.NORMAL;

        //Reset alpha for some case that ship get died so fast cause alpha controlled by shader disrupted
        Color c = shipSprite.color;
        c.a = 1.0f;
        shipSprite.color = c;

        fireJetParticle.Play();
        SetShipPosition();
        shipAnimator.Play(idleAnimHash);

        gun.SetupGun();
    }
    public void TakeDamage(int damage)
    {
        TakeDamageAction?.Invoke();
        currentHP -= damage;
        if (currentHP <= 0)
        {
            InputManager.Instance.DisableInput();
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
        gun.SetupGun();
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

        if(collision.gameObject.CompareTag("Environment") || collision.gameObject.CompareTag("Bullet"))
        {
            return;
        }
        HandleGetHitByEntity(1);
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
    }

    private void HandleCollectCoin(Collision2D coin)
    {
        gameManager.UpdateCoin(coin.gameObject.GetComponent<BaseCoin>().coinValue);
        AudioManager.Instance.PlaySFX(AudioTagName.COLLECT_COIN);
        LeanPool.Despawn(coin.gameObject);
    }

    private void HandleCollectItem(Collision2D item)
    {
        DataManager.Instance.SaveDataToLocalStorage();
        rankManager.UpdateRankPoints(item.gameObject.GetComponent<BaseItem>().m_rank_point);
        AudioManager.Instance.PlaySFX(AudioTagName.COLLECT_COIN);
        LeanPool.Despawn(item.gameObject);
    }


    #endregion Collison Detection

    #region Control ship methods
   
    public void SetShipPosition()
    {
        var postion = new Vector3(0, -screenBounds.y + shipSpriteHeight * 1.5f, 0);
        var target = postion;
        lastPosX = postion.x;
        postion.y -= 3.0f;
        transform.position = postion;
        LeanTween.moveY(gameObject, target.y, 1.2f)
            .setEase(LeanTweenType.easeOutBack)
            .setOnComplete(()=> {
                UIManager.Instance.holdToPlayUI.Show();
            });
    }
    #endregion Control ship methods
}