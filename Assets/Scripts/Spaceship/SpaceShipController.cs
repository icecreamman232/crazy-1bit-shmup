using System.Collections;
using UnityEngine;
using System;
using Lean.Pool;
using Lean.Touch;

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

    #region Touch Configs
    [Header("Touch Configuration")]
    [Range(0, 3)]
    public float sensitivity;
    public bool isTouching;
    public bool firstTouch;
    #endregion Touch Configs



    [Header("Reference")]
    public ParticleSystem fireJetParticle;
    public SpriteRenderer shipSprite;
    public RankManager rankManager;

    private GameManager gameManager;
    public Animator holdToPlayAnimator;
    #region UI

    [Header("UI")]
    public UIHealthBarController uiShipHPBar;

    #endregion UI

    #region SFX

    [Header("SFX")]
    public AudioSource sfx;

    public AudioClip sfxCoinCollect;

    #endregion SFX
    private Camera mainCamera;
    private float shipSpriteWidth;
    private float shipSpriteHeight;
    private float lastPosX;
    private Animator shipAnimator;

    private Vector2 screenBounds;

    private float timer;

    public event Action<ShipStatus> OnDeath;


    private int idleAnimHash;
    private int turnLeftHash;
    private int turnRightHash;

    private void Start()
    {
        gameManager = GameManager.Instance;
        mainCamera = Camera.main;
        shipAnimator = GetComponent<Animator>();
        screenBounds = GameHelper.GetCurrentScreenBounds();

        shipSpriteWidth = shipSprite.bounds.size.x;
        shipSpriteHeight = shipSprite.bounds.size.y;
        
        isTouching = false;
        firstTouch = false;
        

        idleAnimHash = Animator.StringToHash("ship_idle");
        turnLeftHash = Animator.StringToHash("ship_turn_left_anim");
        turnRightHash = Animator.StringToHash("ship_turn_right_anim"); 

    }
    void Update()
    {
        if (currentStatus == ShipStatus.NORMAL || currentStatus == ShipStatus.INVINCIBLE)
        {
            if (!isTouching) return;
            var screenDelta = LeanGesture.GetScreenDelta();
            if (screenDelta == Vector2.zero) return;
            TranslateShip(screenDelta);
        }
    }
    private void OnDestroy()
    {
        OnDisableTouch();
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
        SetShipPosition();
        shipAnimator.Play(idleAnimHash);

        
    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            OnDisableTouch();
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
        uiShipHPBar.UpdateHealthBarUI();
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
    public void OnEnableTouch()
    {
        LeanTouch.OnFingerDown += OnTouchDown;
        LeanTouch.OnFingerUp += OnTouchUp;
    }
    public void OnDisableTouch()
    {
        LeanTouch.OnFingerDown -= OnTouchDown;
        LeanTouch.OnFingerUp -= OnTouchUp;
        isTouching = false;
        firstTouch = false;
    }
    public void OnTouchDown(LeanFinger fingers)
    {
        if (!firstTouch)
        {
            firstTouch = true;
            holdToPlayAnimator.Play("holdtoplay_outtro_anim");
        }
        isTouching = true;
    }
    public void OnTouchUp(LeanFinger fingers)
    {
        isTouching = false;
        if (currentStatus == ShipStatus.NORMAL)
        {
            shipAnimator.Play("ship_idle");
        }

    }
    public void SetShipPosition()
    {
        var postion = new Vector3(0, -screenBounds.y + shipSpriteHeight * 1.5f, 0);
        var target = postion;
        lastPosX = postion.x;
        postion.y -= 3.0f;
        transform.position = postion;
        isTouching = false;
        LeanTween.moveY(gameObject, target.y, 1.2f)
            .setEase(LeanTweenType.easeOutBack)
            .setOnComplete(OnCompleteSetupShipPosition);
    }
    void OnCompleteSetupShipPosition()
    {
        OnEnableTouch();
        holdToPlayAnimator.gameObject.SetActive(true);
        holdToPlayAnimator.Play("holdtoplay_intro_anim");
    }
    void TranslateShip(Vector2 delta)
    {
        if (mainCamera != null)
        {
            var screenPts = mainCamera.WorldToScreenPoint(transform.position);
            screenPts += (Vector3)delta * sensitivity;
            var worldPts = mainCamera.ScreenToWorldPoint(screenPts);


            if (worldPts.x <= -screenBounds.x + shipSpriteWidth * 0.5f) worldPts.x = -screenBounds.x + shipSpriteWidth * 0.5f;
            if (worldPts.x >= screenBounds.x - shipSpriteWidth * 0.5f) worldPts.x = screenBounds.x - shipSpriteWidth * 0.5f;
            if (worldPts.y <= -screenBounds.y) worldPts.y = -screenBounds.y;
            if (worldPts.y >= screenBounds.y) worldPts.y = screenBounds.y;

            //Ship keep turning left
            if (worldPts.x < lastPosX)
            {
                shipAnimator.Play(turnLeftHash);
            }
            //Ship keep turning right
            else if (worldPts.x > lastPosX)
            {
                shipAnimator.Play(turnRightHash);
            }
            else
            {
                shipAnimator.Play(idleAnimHash);
            }

            lastPosX = worldPts.x;
            transform.position = worldPts;
        }
        else
        {
            Debug.LogError("Camera is null");
        }
    }
    #endregion Control ship methods
}