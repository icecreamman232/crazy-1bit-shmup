using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    [Header("Basic Information")]
    public int baseHP;
    public int currentHP;
    public float firerate;
    public int damage;

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
       StartCoroutine(Shoot());
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
        if (timer > 0) return;

        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            GameManager.Instance.cameraShakeFX.Shake();
            StartCoroutine(OnInvincible());
            UpdateHP(1);
            uiShipHPBar.UpdateHealthBarUI();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            GameManager.Instance.UpdateCoin(collision.gameObject.GetComponent<BaseCoin>().coinValue);
            sfx.PlayOneShot(sfxCoinCollect);
            Lean.Pool.LeanPool.Despawn(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Item"))
        {
            DataManager.Instance.SaveDataToLocalStorage();
            rankManager.UpdateRankPoints(collision.gameObject.GetComponent<BaseItem>().m_rank_point);
            sfx.PlayOneShot(sfxCoinCollect);
            Lean.Pool.LeanPool.Despawn(collision.gameObject);
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
}
