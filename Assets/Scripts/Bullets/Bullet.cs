using UnityEngine;

[System.Serializable]
public enum BulletType
{
    SHIP_BULLET = 0,
    MONSTER_BULLET = 1,
}

public class Bullet : MonoBehaviour
{
    public BulletType type;

    [SerializeField]
    private int damage;

    [SerializeField]
    private float bulletMoveSpeed;

    #region Getter Setter

    public int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
            if (damage < 0)
            {
                damage = 0;
#if UNITY_EDITOR
                Debug.Log("Damage is negative");
#endif
            }
        }
    }

    public float MoveSpeed
    {
        get
        {
            return bulletMoveSpeed;
        }
        set
        {
            bulletMoveSpeed = value;
            if (bulletMoveSpeed < 0)
            {
                bulletMoveSpeed = 0;
#if UNITY_EDITOR
                Debug.Log("Bullet MoveSpeed is negative");
#endif
            }
        }
    }

    #endregion Getter Setter

    protected Vector3 originPos;
    protected Rigidbody2D rigidBody;
    protected Animator animator;

    private float screenBoundX;
    private float screenBoundY;

    private void Awake()
    {
        originPos = transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        screenBoundX = GameHelper.GetCurrentScreenBounds().x;
        screenBoundY = GameHelper.GetCurrentScreenBounds().y;
    }

    public void Update()
    {
        if (transform.position.y >= screenBoundY)
        {
            ResetBullet();
            return;
        }
        else if (transform.position.y <= -screenBoundY)
        {
            ResetBullet();
            return;
        }
        else if (transform.position.x >= screenBoundX + 1.0f)
        {
            ResetBullet();
            return;
        }
        else if (transform.position.x <= -screenBoundX - 1.0f)
        {
            ResetBullet();
            return;
        }
    }

    public virtual void Shoot(Vector3 rotation)
    {
    }

    public virtual void ResetBullet()
    {
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
    }
}